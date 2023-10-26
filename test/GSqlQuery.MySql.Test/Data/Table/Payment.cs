using System;

namespace GSqlQuery.MySql.Test.Data.Table
{
    [Table("sakila", "payment")]
    public class Payment : EntityExecute<Payment>
    {
        [Column("payment_id", Size = 5, IsPrimaryKey = true, IsAutoIncrementing = true)]
        public long PaymentId { get; set; }

        [Column("customer_id", Size = 5)]
        public long CustomerId { get; set; }

        [Column("staff_id", Size = 3)]
        public byte StaffId { get; set; }

        [Column("rental_id", Size = 10)]
        public int? RentalId { get; set; }

        [Column("amount", Size = 5)]
        public decimal Amount { get; set; }

        [Column("payment_date", Size = 0)]
        public DateTime PaymentDate { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime? LastUpdate { get; set; }

        public Payment()
        { }

        public Payment(long paymentId, long customerId, byte staffId, int? rentalId, decimal amount, DateTime paymentDate, DateTime? lastUpdate)
        {
            PaymentId = paymentId;
            CustomerId = customerId;
            StaffId = staffId;
            RentalId = rentalId;
            Amount = amount;
            PaymentDate = paymentDate;
            LastUpdate = lastUpdate;
        }
    }
}
