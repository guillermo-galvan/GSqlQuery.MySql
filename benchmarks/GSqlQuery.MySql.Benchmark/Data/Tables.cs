namespace GSqlQuery.MySql.Benchmark.Data
{
    [Table("information_schema", "tables")]
    internal class Tables : EntityExecute<Tables>
    {
        [Column("table_schema")]
        public string TableSchema { get; set; }

        [Column("table_name")]
        public string Name { get; set; }

        public Tables()
        { }

        public Tables(string tableSchema, string name)
        {
            TableSchema = tableSchema;
            Name = name;
        }
    }
}