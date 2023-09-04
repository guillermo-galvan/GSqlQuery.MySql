namespace GSqlQuery.MySql.BulkCopy
{
    public static class BulkCopyFactory
    {
        public static IMySqlBulkCopy Create(string connectionString)
        {
            return new BulkCopyExecute(connectionString);
        }

        public static IMySqlBulkCopy Create(string connectionString, IStatements statements)
        {
            return new BulkCopyExecute(connectionString, statements);
        }
    }
}