using GSqlQuery.Runner;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql
{
    public sealed class MySqlDatabaseConnection : Connection, IConnection
    {
        public MySqlDatabaseConnection(string connectionString) : base(new MySqlConnection(connectionString))
        { }

        public MySqlDatabaseTransaction BeginTransaction()
        {
            return (MySqlDatabaseTransaction)SetTransaction(new MySqlDatabaseTransaction(this, ((MySqlConnection)_connection).BeginTransaction()));
        }

        public MySqlDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return (MySqlDatabaseTransaction)SetTransaction(new MySqlDatabaseTransaction(this, ((MySqlConnection)_connection).BeginTransaction(isolationLevel)));
        }

        public async Task<MySqlDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return (MySqlDatabaseTransaction)SetTransaction(new MySqlDatabaseTransaction(this, await ((MySqlConnection)_connection).BeginTransactionAsync(cancellationToken)));
        }

        public async Task<MySqlDatabaseTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return
                (MySqlDatabaseTransaction)SetTransaction(new MySqlDatabaseTransaction(this,
                await ((MySqlConnection)_connection).BeginTransactionAsync(isolationLevel, cancellationToken)));
        }

        public override Task CloseAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ((MySqlConnection)_connection).CloseAsync();
        }

        ITransaction IConnection.BeginTransaction() => BeginTransaction();

        ITransaction IConnection.BeginTransaction(IsolationLevel isolationLevel) => BeginTransaction(isolationLevel);

        async Task<ITransaction> IConnection.BeginTransactionAsync(CancellationToken cancellationToken = default) => await BeginTransactionAsync(cancellationToken);

        async Task<ITransaction> IConnection.BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default) =>
            await BeginTransactionAsync(isolationLevel, cancellationToken);

        ~MySqlDatabaseConnection()
        {
            Dispose(disposing: false);
        }
    }
}