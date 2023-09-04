using GSqlQuery.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.BulkCopy
{
    internal class BulkCopyExecute : IMySqlBulkCopyExecute
    {
        const string ALLOWLOADLOCALINFILE = "ALLOWLOADLOCALINFILE=TRUE";
        const string ALLOWUSERVARIABLES = "ALLOWUSERVARIABLES=TRUE";
        const string FieldTerminator = "^";

        private readonly Queue<FileBulkLoader> _files;
        private readonly string _connectionString;
        private readonly IStatements _statements;
        private uint _boolCount = 1;
        private bool _localInfileModify = false;

        public IDatabaseManagement<MySqlDatabaseConnection> DatabaseManagement { get; }

        IDatabaseManagement<MySqlConnection> IExecute<int, MySqlConnection>.DatabaseManagement => throw new NotImplementedException();

        public BulkCopyExecute(string connectionString) : this(connectionString, new MySqlStatements())
        { }

        public BulkCopyExecute(string connectionString, IStatements statements)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (!connectionString.ToUpper().Contains(ALLOWLOADLOCALINFILE))
            {
                throw new InvalidOperationException("Connection string does not contain AllowLoadLocalInfile=true");
            }

            if (!connectionString.ToUpper().Contains(ALLOWUSERVARIABLES))
            {
                throw new InvalidOperationException("Connection string does not contain AllowUserVariables=true");
            }

            _connectionString = connectionString;
            _files = new Queue<FileBulkLoader>();
            _statements = statements ?? throw new ArgumentNullException(nameof(statements));
            DatabaseManagement = new MySqlDatabaseManagement(_connectionString);
        }

        public IMySqlBulkCopyExecute Copy<T>(IEnumerable<T> values)
        {
            if (values == null && !values.Any())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            _files.Enqueue(CreateFileBulkLoader(values));
            return this;
        }

        public int Execute()
        {
            int result = 0;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                result = Execute(connection);

                connection.Close();
            }

            return result;
        }

        public int Execute(MySqlConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            int result = 0;

            try
            {
                LocalInfileVerify(connection);

                foreach (var item in _files)
                {
                    result += WriteToBulkCopy(connection, item);
                }
            }
            finally
            {
                LocalInfileVerify(connection, false);
            }

            return result;
        }

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            int result = 0;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                result = await ExecuteAsync(connection, cancellationToken);

                await connection.CloseAsync();
            }

            return result;
        }

        public async Task<int> ExecuteAsync(MySqlConnection connection, CancellationToken cancellationToken = default)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            cancellationToken.ThrowIfCancellationRequested();
            int result = 0;

            try
            {
                LocalInfileVerify(connection);

                foreach (var item in _files)
                {
                    result += await WriteToBulkCopyAsync(connection, item, cancellationToken);
                }
            }
            finally
            {
                LocalInfileVerify(connection, false);
            }

            return result;
        }

        private FileBulkLoader CreateFileBulkLoader<T>(IEnumerable<T> values)
        {
            var classOption = ClassOptionsFactory.GetClassOptions(typeof(T));

            List<string> columns = new List<string>();
            List<string> expressions = new List<string>();

            foreach (PropertyOptions property in classOption.PropertyOptions)
            {
                if (!property.ColumnAttribute.IsAutoIncrementing)
                {
                    if (property.PropertyInfo.PropertyType == typeof(bool) || property.PropertyInfo.PropertyType == typeof(bool?))
                    {
                        var boolName = $"@var{_boolCount++}";

                        columns.Add(boolName);

                        expressions.Add($"{property.ColumnAttribute.Name} = ({boolName} = 'True');");

                    }
                    else
                    {
                        columns.Add(property.ColumnAttribute.Name);
                    }
                }
            }

            string path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (var item in values)
                {
                    Queue<object> fields = new Queue<object>();

                    foreach (PropertyOptions property in classOption.PropertyOptions)
                    {
                        if (!property.ColumnAttribute.IsAutoIncrementing)
                        {
                            object val;

                            if (property.PropertyInfo.PropertyType == typeof(DateTime) || property.PropertyInfo.PropertyType == typeof(DateTime?))
                            {
                                DateTime? date = (DateTime?)property.PropertyInfo.GetValue(item);
                                val = date?.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (property.PropertyInfo.PropertyType == typeof(bool) || property.PropertyInfo.PropertyType == typeof(bool?))
                            {
                                bool? data = (bool?)property.PropertyInfo.GetValue(item);
                                val = data.HasValue ? data.Value ? "True" : "False" : null;
                            }
                            else
                            {
                                val = property.PropertyInfo.GetValue(item) ?? null;
                            }

                            fields.Enqueue(val);
                        }
                    }
                    sw.Write(string.Join(FieldTerminator, fields));
                    sw.Write(Environment.NewLine);
                }

                sw.Flush();
                sw.Close();
            }

            return new FileBulkLoader(classOption.Table.GetTableName(_statements), path, columns, expressions);
        }

        private void LocalInfileVerify(MySqlConnection connection, bool isValidation = true)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                if (isValidation)
                {
                    command.CommandText = "SELECT @@GLOBAL.local_infile;";
                    _localInfileModify = (long)command.ExecuteScalar() == 0;

                    if (_localInfileModify)
                    {
                        command.CommandText = "SET GLOBAL local_infile=1;";
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    if (_localInfileModify)
                    {
                        command.CommandText = "SET GLOBAL local_infile=0;";
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private MySqlBulkLoader GetMySqlBulkLoader(MySqlConnection mySqlConnection, FileBulkLoader fileBulkLoader)
        {
            MySqlBulkLoader loader = new MySqlBulkLoader(mySqlConnection)
            {
                Local = true,
                TableName = fileBulkLoader.TableName,
                FileName = fileBulkLoader.Path,
                FieldTerminator = FieldTerminator,
                LineTerminator = Environment.NewLine,

            };

            loader.Columns.AddRange(fileBulkLoader.Columns);
            loader.Expressions.AddRange(fileBulkLoader.Expressions);

            return loader;
        }

        private int WriteToBulkCopy(MySqlConnection mySqlConnection, FileBulkLoader fileBulkLoader)
        {
            return GetMySqlBulkLoader(mySqlConnection, fileBulkLoader).Load();
        }

        private Task<int> WriteToBulkCopyAsync(MySqlConnection mySqlConnection, FileBulkLoader fileBulkLoader, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return GetMySqlBulkLoader(mySqlConnection, fileBulkLoader).LoadAsync();
        }

        IBulkCopyExecute IBulkCopy.Copy<T>(IEnumerable<T> values) => Copy(values);
    }
}