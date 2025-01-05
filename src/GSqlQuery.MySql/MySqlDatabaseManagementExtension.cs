using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql
{
    public static class MySqlDatabaseManagementExtension
    {
        public static TResult ExecuteWithTransaction<TResult>(this IExecute<TResult, MySqlDatabaseConnection> query)
        {
            using (MySqlDatabaseConnection connection = query.DatabaseManagement.GetConnection())
            {
                using (MySqlDatabaseTransaction transaction = connection.BeginTransaction())
                {
                    TResult result = query.Execute(transaction.Connection);
                    transaction.Commit();
                    connection.Close();
                    return result;
                }
            }
        }

        public static TResult ExecuteWithTransaction<TResult>(this IExecute<TResult, MySqlDatabaseConnection> query, MySqlDatabaseTransaction transaction)
        {
            return query.Execute(transaction.Connection);
        }

        public static async Task<TResult> ExecuteWithTransactionAsync<TResult>(this IExecute<TResult, MySqlDatabaseConnection> query, CancellationToken cancellationToken = default)
        {
            using (MySqlDatabaseConnection connection = await query.DatabaseManagement.GetConnectionAsync(cancellationToken).ConfigureAwait(false))
            {
                using (MySqlDatabaseTransaction transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false))
                {
                    TResult result = await query.ExecuteAsync(transaction.Connection, cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    await connection.CloseAsync(cancellationToken);
                    return result;
                }
            }
        }

        public static Task<TResult> ExecuteWithTransactionAsync<TResult>(this IExecute<TResult, MySqlDatabaseConnection> query, MySqlDatabaseTransaction transaction,
            CancellationToken cancellationToken = default)
        {
            return query.ExecuteAsync(transaction.Connection, cancellationToken);
        }
    }
}