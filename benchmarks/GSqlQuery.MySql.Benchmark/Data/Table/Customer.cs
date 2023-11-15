using System;

namespace GSqlQuery.MySql.Benchmark.Data.Table
{
    [Table("sakila", "customer")]
    public class Customer : EntityExecute<Customer>
    {
        [Column("customer_id", Size = 5, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public long CustomerId { get; set; }

        [Column("store_id", Size = 3)]
        public byte StoreId { get; set; }

        [Column("first_name", Size = 45)]
        public string FirstName { get; set; }

        [Column("last_name", Size = 45)]
        public string LastName { get; set; }

        [Column("email", Size = 50)]
        public string Email { get; set; }

        [Column("address_id", Size = 5)]
        public long AddressId { get; set; }

        [Column("active", Size = 3)]
        public byte Active { get; set; }

        [Column("create_date", Size = 19)]
        public DateTime CreateDate { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime? LastUpdate { get; set; }

        public Customer()
        { }

        public Customer(long customerId, byte storeId, string firstName, string lastName, string email, long addressId, byte active, DateTime createDate, DateTime? lastUpdate)
        {
            CustomerId = customerId;
            StoreId = storeId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            AddressId = addressId;
            Active = active;
            CreateDate = createDate;
            LastUpdate = lastUpdate;
        }
    }
}
