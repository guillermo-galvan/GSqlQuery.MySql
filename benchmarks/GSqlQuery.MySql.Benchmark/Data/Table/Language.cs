using System;

namespace GSqlQuery.MySql.Benchmark.Data.Table
{
    [Table("sakila", "language")]
    public class Language : EntityExecute<Language>
    {
        [Column("language_id", Size = 3, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public byte LanguageId { get; set; }

        [Column("name", Size = 20)]
        public string Name { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime LastUpdate { get; set; }

        public Language()
        { }

        public Language(byte languageId, string name, DateTime lastUpdate)
        {
            LanguageId = languageId;
            Name = name;
            LastUpdate = lastUpdate;
        }
    }
}
