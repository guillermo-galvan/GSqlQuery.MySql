using GSqlQuery.Runner;
using MySql.Data.MySqlClient;

namespace GSqlQuery.MySql
{
    public sealed class MySqlDatabaseTransaction : Transaction
    {
        public MySqlDatabaseTransaction(MySqlDatabaseConnection mySqlDatabaseConnection, MySqlTransaction mySqlTransaction)
            : base(mySqlDatabaseConnection, mySqlTransaction)
        { }

        public MySqlDatabaseConnection Connection => (MySqlDatabaseConnection)_connection;

        public MySqlTransaction Transaction => (MySqlTransaction)_transaction;

        ~MySqlDatabaseTransaction()
        {
            Dispose(disposing: false);
        }
    }
}