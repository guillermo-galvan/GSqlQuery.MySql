using System;

namespace GSqlQuery.MySql.Benchmark.Data.Table
{
    [Table("sakila", "city")]
    public class City : EntityExecute<City>
    {
        [Column("city_id", Size = 5, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public long CityId { get; set; }

        [Column("city", Size = 50)]
        public string Name { get; set; }

        [Column("country_id", Size = 5)]
        public long CountryId { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime LastUpdate { get; set; }

        public City()
        { }

        public City(long cityId, string name, long countryId, DateTime lastUpdate)
        {
            CityId = cityId;
            Name = name;
            CountryId = countryId;
            LastUpdate = lastUpdate;
        }
    }
}
