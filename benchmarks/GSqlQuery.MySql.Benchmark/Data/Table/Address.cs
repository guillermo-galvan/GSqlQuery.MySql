using MySql.Data.Types;
using System;

namespace GSqlQuery.MySql.Benchmark.Data.Table
{
    [Table("sakila", "address")]
    public class Address : EntityExecute<Address>
    {
        [Column("address_id", Size = 5, IsAutoIncrementing = true,IsPrimaryKey = true)]
        public long AddressId { get; set; }

        [Column("address", Size = 50)]
        public string Address1 { get; set; }
        
        [Column("address2", Size = 50)]
        public string Address2 { get; set; }

        [Column("district", Size = 20)]
        public string District { get; set; }

        [Column("city_id", Size = 5)]
        public long CityId { get; set; }

        [Column("postal_code", Size = 10)]
        public string PostalCode { get; set; }

        [Column("phone", Size = 20)]
        public string Phone { get; set; }

        [Column("location")]
        public MySqlGeometry Location { get; set; }

        [Column("last_update", Size = 50)]
        public DateTime LastUpdate { get; set; }

        public Address()
        { }

        public Address(long addressId, string address1, string address2, string district, long cityId, string postalCode, string phone, MySqlGeometry location, DateTime lastUpdate)
        {
            AddressId = addressId;
            Address1 = address1;
            Address2 = address2;
            District = district;
            CityId = cityId;
            PostalCode = postalCode;
            Phone = phone;
            Location = location;
            LastUpdate = lastUpdate;
        }
    }
}
