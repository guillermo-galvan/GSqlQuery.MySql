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
    internal class BulkCopyExecute(BulkCopyConfiguration bulkCopyConfiguration) : IMySqlBulkCopyExecute
    {
        private readonly Queue<FileBulkLoader> _files = new Queue<FileBulkLoader>();
        private bool _localInfileModify = false;
        private readonly BulkCopyConfiguration _bulkCopyConfiguration = bulkCopyConfiguration ?? throw new ArgumentNullException(nameof(bulkCopyConfiguration));

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

            using (MySqlConnection connection = new MySqlConnection(_bulkCopyConfiguration.ConnectionString))
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

            List<int> result = new List<int>();

            try
            {
                LocalInfileVerify(connection);

                foreach (FileBulkLoader item in _files)
                {
                    result.Add( WriteToBulkCopy(connection, item));
                }
            }
            finally
            {
                LocalInfileVerify(connection, false);
            }

            return result.Sum();
        }

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            int result = 0;

            using (MySqlConnection connection = new MySqlConnection(_bulkCopyConfiguration.ConnectionString))
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

                foreach (FileBulkLoader item in _files)
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
            ClassOptions classOption = ClassOptionsFactory.GetClassOptions(typeof(T));

            List<string> columns = [];
            List<string> expressions = [];

            foreach (KeyValuePair<string, PropertyOptions> property in classOption.PropertyOptions)
            {
                if (!property.Value.ColumnAttribute.IsAutoIncrementing)
                {
                    ColumnAndExpression columnsAndExpression = _bulkCopyConfiguration.Events.GetColumnaAndExpression(property.Value);

                    if (string.IsNullOrEmpty(columnsAndExpression.ColumnName))
                    {
                        throw new InvalidOperationException("Column name cannot be null");
                    }

                    columns.Add(columnsAndExpression.ColumnName);

                    if (!string.IsNullOrEmpty(columnsAndExpression.Expression))
                    {
                        expressions.Add(columnsAndExpression.Expression);
                    }
                }
            }

            string path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (T item in values)
                {
                    Queue<object> fields = new Queue<object>();

                    foreach (KeyValuePair<string, PropertyOptions> property in classOption.PropertyOptions)
                    {
                        if (!property.Value.ColumnAttribute.IsAutoIncrementing)
                        {
                            fields.Enqueue(_bulkCopyConfiguration.Events.Format(property.Value, property.Value.PropertyInfo.GetValue(item)));
                        }
                    }
                    sw.Write(string.Join(BulkCopyConfiguration.FIELDTERMINATOR, fields));
                    sw.Write(Environment.NewLine);
                }

                sw.Flush();
                sw.Close();
            }

            return new FileBulkLoader(classOption.FormatTableName.GetTableName(_bulkCopyConfiguration.Formats), path, columns, expressions);
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
                FieldTerminator = BulkCopyConfiguration.FIELDTERMINATOR,
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
    }
}