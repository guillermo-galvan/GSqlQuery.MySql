using MySql.Data.MySqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.BulkCopy
{
    public interface IMySqlBulkCopyExecute : IMySqlBulkCopy, IBulkCopyExecute
    {
        int Execute(MySqlConnection dbConnection);

        Task<int> ExecuteAsync(MySqlConnection dbConnection, CancellationToken cancellationToken = default(CancellationToken));
    }
}