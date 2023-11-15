namespace GSqlQuery.MySql.BulkCopy
{
    public static class BulkCopyFactory
    {
        public static IMySqlBulkCopy Create(string connectionString)
        {
            return new BulkCopyExecute(new BulkCopyConfiguration(connectionString));
        }

        public static IMySqlBulkCopy Create(BulkCopyConfiguration bulkCopyConfiguration)
        {
            return new BulkCopyExecute(bulkCopyConfiguration);
        }
    }
}