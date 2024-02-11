using GSqlQuery.Runner;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql
{
    public sealed class MySqlDatabaseConnection(string connectionString) :
        Connection<MySqlDatabaseTransaction, MySqlConnection, MySqlTransaction, MySqlCommand>(new MySqlConnection(connectionString)) , 
        IConnection<MySqlDatabaseTransaction,MySqlCommand>
    {
        public override Task CloseAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _connection.CloseAsync();
        }

        public override MySqlDatabaseTransaction BeginTransaction()
        {
            MySqlTransaction mySqlTransaction = _connection.BeginTransaction();
            return SetTransaction(new MySqlDatabaseTransaction(this, mySqlTransaction));
        }

        public override MySqlDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return SetTransaction(new MySqlDatabaseTransaction(this, _connection.BeginTransaction(isolationLevel)));
        }

        public async override Task<MySqlDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MySqlTransaction mySqlTransaction = await _connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            return SetTransaction(new MySqlDatabaseTransaction(this, mySqlTransaction));
        }

        public async override Task<MySqlDatabaseTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MySqlTransaction mySqlTransaction = await _connection.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
            return SetTransaction(new MySqlDatabaseTransaction(this, mySqlTransaction));
        }

        ~MySqlDatabaseConnection()
        {
            Dispose(disposing: false);
        }
    }
}