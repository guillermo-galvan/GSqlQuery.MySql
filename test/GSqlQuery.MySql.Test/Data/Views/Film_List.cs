namespace GSqlQuery.MySql.Test.Data.Views
{
    [Table("sakila", "film_list")]
    public class Film_List : Entity<Film_List>
    {
        [Column("FID", Size = 5)]
        public long? FID { get; set; }

        [Column("title", Size = 255)]
        public string Title { get; set; }

        [Column("description", Size = 65535)]
        public string Description { get; set; }

        [Column("category", Size = 25)]
        public string Category { get; set; }

        [Column("price", Size = 4)]
        public decimal? Price { get; set; }

        [Column("length", Size = 5)]
        public long? Length { get; set; }

        [Column("rating", Size = 5)]
        public string Rating { get; set; }

        [Column("actors", Size = 65535)]
        public string Actors { get; set; }

        public Film_List()
        { }

        public Film_List(long? fID, string title, string description, string category, decimal? price, long? length, string rating, string actors)
        {
            FID = fID;
            Title = title;
            Description = description;
            Category = category;
            Price = price;
            Length = length;
            Rating = rating;
            Actors = actors;
        }
    }
}
