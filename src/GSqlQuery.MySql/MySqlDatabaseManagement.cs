using GSqlQuery.Runner;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql
{
    public class MySqlDatabaseManagement :
        DatabaseManagement<MySqlDatabaseConnection, MySqlDatabaseTransaction, MySqlCommand, MySqlTransaction, MySqlDataReader>, 
        IDatabaseManagement<MySqlDatabaseConnection>
    {
        public MySqlDatabaseManagement(string connectionString) :
            base(connectionString, new MySqlDatabaseManagementEvents())
        { }

        public MySqlDatabaseManagement(string connectionString, DatabaseManagementEvents events) : base(connectionString, events)
        { }

        public override MySqlDatabaseConnection GetConnection()
        {
            MySqlDatabaseConnection mySqlDatabase = new MySqlDatabaseConnection(_connectionString);

            if (mySqlDatabase.State != ConnectionState.Open)
            {
                mySqlDatabase.Open();
            }

            return mySqlDatabase;
        }

        public async override Task<MySqlDatabaseConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MySqlDatabaseConnection databaseConnection = new MySqlDatabaseConnection(_connectionString);

            if (databaseConnection.State != ConnectionState.Open)
            {
                await databaseConnection.OpenAsync(cancellationToken);
            }

            return databaseConnection;
        }
    }
}