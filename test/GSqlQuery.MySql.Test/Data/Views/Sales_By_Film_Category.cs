namespace GSqlQuery.MySql.Test.Data.Views
{
    [Table("sakila", "sales_by_film_category")]
    public class Sales_By_Film_Category : Entity<Sales_By_Film_Category>
    {
        [Column("category", Size = 25)]
        public string Category { get; set; }

        [Column("total_sales", Size = 27)]
        public decimal? TotalSales { get; set; }

        public Sales_By_Film_Category()
        { }

        public Sales_By_Film_Category(string category, decimal? totalSales)
        {
            Category = category;
            TotalSales = totalSales;
        }
    }
}
