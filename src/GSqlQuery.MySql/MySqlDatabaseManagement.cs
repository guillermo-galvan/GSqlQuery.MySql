using GSqlQuery.Runner;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql
{
    public sealed class MySqlDatabaseManagement : DatabaseManagement, IDatabaseManagement<MySqlDatabaseConnection>
    {
        public MySqlDatabaseManagement(string connectionString) :
            base(connectionString, new MySqlDatabaseManagementEvents())
        { }

        public MySqlDatabaseManagement(string connectionString, DatabaseManagementEvents events) : base(connectionString, events)
        { }

        public MySqlDatabaseManagement(string connectionString, DatabaseManagementEvents events, ILogger logger) : base(connectionString, events, logger)
        { }

        public int ExecuteNonQuery(MySqlDatabaseConnection connection, IQuery query, IEnumerable<IDataParameter> parameters)
        {
            return base.ExecuteNonQuery(connection, query, parameters);
        }

        public Task<int> ExecuteNonQueryAsync(MySqlDatabaseConnection connection, IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            return base.ExecuteNonQueryAsync(connection, query, parameters, cancellationToken);
        }

        public IEnumerable<T> ExecuteReader<T>(MySqlDatabaseConnection connection, IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters) 
            where T : class, new()
        {
            return base.ExecuteReader<T>(connection, query, propertyOptions, parameters);
        }

        public Task<IEnumerable<T>> ExecuteReaderAsync<T>(MySqlDatabaseConnection connection, IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default) 
            where T : class, new()
        {
            return base.ExecuteReaderAsync<T>(connection, query, propertyOptions, parameters, cancellationToken);
        }

        public T ExecuteScalar<T>(MySqlDatabaseConnection connection, IQuery query, IEnumerable<IDataParameter> parameters)
        {
            return base.ExecuteScalar<T>(connection, query, parameters);
        }

        public Task<T> ExecuteScalarAsync<T>(MySqlDatabaseConnection connection, IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            return base.ExecuteScalarAsync<T>(connection, query, parameters, cancellationToken);
        }

        public override IConnection GetConnection()
        {
            MySqlDatabaseConnection mySqlDatabase = new MySqlDatabaseConnection(_connectionString);

            if (mySqlDatabase.State != ConnectionState.Open)
            {
                mySqlDatabase.Open();
            }

            return mySqlDatabase;
        }

        MySqlDatabaseConnection IDatabaseManagement<MySqlDatabaseConnection>.GetConnection()
        {
            return (MySqlDatabaseConnection)GetConnection();
        }

        public async override Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            MySqlDatabaseConnection databaseConnection = new MySqlDatabaseConnection(_connectionString);

            if (databaseConnection.State != ConnectionState.Open)
            {
                await databaseConnection.OpenAsync(cancellationToken);
            }

            return databaseConnection;
        }

        async Task<MySqlDatabaseConnection> IDatabaseManagement<MySqlDatabaseConnection>.GetConnectionAsync(CancellationToken cancellationToken)
        {
            return (MySqlDatabaseConnection)await GetConnectionAsync(cancellationToken);
        }
    }
}