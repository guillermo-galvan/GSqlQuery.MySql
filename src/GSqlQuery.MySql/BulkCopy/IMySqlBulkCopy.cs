using System.Collections.Generic;

namespace GSqlQuery.MySql.BulkCopy
{
    public interface IMySqlBulkCopy
    {
        IMySqlBulkCopyExecute Copy<T>(IEnumerable<T> values);
    }
}