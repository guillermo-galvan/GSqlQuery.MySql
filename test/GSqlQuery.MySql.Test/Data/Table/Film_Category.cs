using System;

namespace GSqlQuery.MySql.Test.Data.Table
{
    [Table("sakila", "film_category")]
    public class Film_Category : EntityExecute<Film_Category>
    {
        [Column("film_id", Size = 5, IsPrimaryKey = true)]
        public long FilmId { get; set; }

        [Column("category_id", Size = 3, IsPrimaryKey = true)]
        public byte CategoryId { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime LastUpdate { get; set; }

        public Film_Category()
        { }

        public Film_Category(long filmId, byte categoryId, DateTime lastUpdate)
        {
            FilmId = filmId;
            CategoryId = categoryId;
            LastUpdate = lastUpdate;
        }
    }
}
