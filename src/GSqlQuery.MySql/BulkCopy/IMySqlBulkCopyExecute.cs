using MySql.Data.MySqlClient;

namespace GSqlQuery.MySql.BulkCopy
{
    public interface IMySqlBulkCopyExecute : IMySqlBulkCopy, IBulkCopyExecute, IExecute<int, MySqlConnection>
    {
    }
}