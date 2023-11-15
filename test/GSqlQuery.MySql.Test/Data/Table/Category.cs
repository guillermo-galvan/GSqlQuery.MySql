using System;

namespace GSqlQuery.MySql.Test.Data.Table
{
    [Table("sakila", "category")]
    public class Category : EntityExecute<Category>
    {
        [Column("category_id", Size = 3, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public byte CategoryId { get; set; }

        [Column("name", Size = 25)]
        public string Name { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime LastUpdate { get; set; }

        public Category()
        { }

        public Category(byte categoryId, string name, DateTime lastUpdate)
        {
            CategoryId = categoryId;
            Name = name;
            LastUpdate = lastUpdate;
        }
    }
}
