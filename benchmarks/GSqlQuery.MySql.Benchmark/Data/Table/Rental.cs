using System;

namespace GSqlQuery.MySql.Benchmark.Data.Table
{
    [Table("sakila", "rental")]
    public class Rental : EntityExecute<Rental>
    {
        [Column("rental_id", Size = 10, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public int RentalId { get; set; }

        [Column("rental_date", Size = 19)]
        public DateTime RentalDate { get; set; }

        [Column("inventory_id", Size = 7)]
        public int InventoryId { get; set; }

        [Column("customer_id", Size = 5)]
        public long CustomerId { get; set; }

        [Column("return_date", Size = 19)]
        public DateTime? ReturnDate { get; set; }

        [Column("staff_id", Size = 3)]
        public byte StaffId { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime LastUpdate { get; set; }

        public Rental()
        { }

        public Rental(int rentalId, DateTime rentalDate, int inventoryId, long customerId, DateTime? returnDate, byte staffId, DateTime lastUpdate)
        {
            RentalId = rentalId;
            RentalDate = rentalDate;
            InventoryId = inventoryId;
            CustomerId = customerId;
            ReturnDate = returnDate;
            StaffId = staffId;
            LastUpdate = lastUpdate;
        }
    }
}
