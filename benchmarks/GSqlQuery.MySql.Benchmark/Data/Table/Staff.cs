using System;

namespace GSqlQuery.MySql.Benchmark.Data.Table
{
    [Table("sakila", "staff")]
    public class Staff : EntityExecute<Staff>
    {
        [Column("staff_id", Size = 3, IsPrimaryKey = true, IsAutoIncrementing = true)]
        public byte StaffId { get; set; }

        [Column("first_name", Size = 45)]
        public string FirstName { get; set; }

        [Column("last_name", Size = 45)]
        public string LastName { get; set; }

        [Column("address_id", Size = 5)]
        public long AddressId { get; set; }

        [Column("picture", Size = 65535)]
        public byte[] Picture { get; set; }

        [Column("email", Size = 50)]
        public string Email { get; set; }

        [Column("store_id", Size = 3)]
        public byte StoreId { get; set; }

        [Column("active", Size = 3)]
        public byte Active { get; set; }

        [Column("username", Size = 16)]
        public string Username { get; set; }

        [Column("password", Size = 40)]
        public string Password { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime LastUpdate { get; set; }

        public Staff()
        { }

        public Staff(byte staffId, string firstName, string lastName, long addressId, byte[] picture, string email, byte storeId, byte active, string username, string password, DateTime lastUpdate)
        {
            StaffId = staffId;
            FirstName = firstName;
            LastName = lastName;
            AddressId = addressId;
            Picture = picture;
            Email = email;
            StoreId = storeId;
            Active = active;
            Username = username;
            Password = password;
            LastUpdate = lastUpdate;
        }
    }
}
