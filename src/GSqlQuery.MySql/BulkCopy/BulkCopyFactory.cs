namespace GSqlQuery.MySql.BulkCopy
{
    public static class BulkCopyFactory
    {
        public static IMySqlBulkCopy Create(string connectionString)
        {
            return new BulkCopyExecute(connectionString);
        }

        public static IMySqlBulkCopy Create(string connectionString, IFormats formats)
        {
            return new BulkCopyExecute(connectionString, formats);
        }
    }
}