using GSqlQuery.Runner;
using MySql.Data.MySqlClient;

namespace GSqlQuery.MySql
{
    public sealed class MySqlDatabaseTransaction(MySqlDatabaseConnection connection, MySqlTransaction transaction) : 
        Transaction<MySqlDatabaseConnection,MySqlCommand, MySqlTransaction, MySqlConnection>(connection, transaction), 
        ITransaction<MySqlDatabaseConnection, MySqlTransaction>
    {
        ~MySqlDatabaseTransaction()
        {
            Dispose(disposing: false);
        }
    }
}