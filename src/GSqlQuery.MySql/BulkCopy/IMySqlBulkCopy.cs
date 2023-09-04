using System.Collections.Generic;

namespace GSqlQuery.MySql.BulkCopy
{
    public interface IMySqlBulkCopy : IBulkCopy
    {
        new IMySqlBulkCopyExecute Copy<T>(IEnumerable<T> values);
    }
}