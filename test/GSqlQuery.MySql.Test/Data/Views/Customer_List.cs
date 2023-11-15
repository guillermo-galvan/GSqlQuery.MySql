namespace GSqlQuery.MySql.Test.Data.Views
{
    [Table("sakila", "customer_list")]
    public class Customer_List : Entity<Customer_List>
    {
        [Column("ID", Size = 5)]
        public long ID { get; set; }

        [Column("name", Size = 91)]
        public string Name { get; set; }

        [Column("address", Size = 50)]
        public string Address { get; set; }

        [Column("zip code", Size = 10)]
        public string Zipcode { get; set; }

        [Column("phone", Size = 20)]
        public string Phone { get; set; }

        [Column("city", Size = 50)]
        public string City { get; set; }

        [Column("country", Size = 50)]
        public string Country { get; set; }

        [Column("notes", Size = 6)]
        public string Notes { get; set; }

        [Column("SID", Size = 3)]
        public byte SID { get; set; }

        public Customer_List()
        { }

        public Customer_List(long iD, string name, string address, string zipcode, string phone, string city, string country, string notes, byte sID)
        {
            ID = iD;
            Name = name;
            Address = address;
            Zipcode = zipcode;
            Phone = phone;
            City = city;
            Country = country;
            Notes = notes;
            SID = sID;
        }
    }
}
