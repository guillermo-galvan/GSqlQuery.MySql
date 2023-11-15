using System;

namespace GSqlQuery.MySql.Test.Data.Table
{
    [Table("sakila", "country")]
    public class Country : EntityExecute<Country>
    {
        [Column("country_id", Size = 5, IsPrimaryKey = true, IsAutoIncrementing = true)]
        public long CountryId { get; set; }

        [Column("country", Size = 50)]
        public string Name { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime LastUpdate { get; set; }

        public Country()
        { }

        public Country(long countryId, string name, DateTime lastUpdate)
        {
            CountryId = countryId;
            Name = name;
            LastUpdate = lastUpdate;
        }
    }
}
