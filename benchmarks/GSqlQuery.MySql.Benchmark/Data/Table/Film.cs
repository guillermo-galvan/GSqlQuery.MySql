using System;

namespace GSqlQuery.MySql.Benchmark.Data.Table
{
    [Table("sakila", "film")]
    public class Film : EntityExecute<Film>
    {
        [Column("film_id", Size = 5, IsPrimaryKey = true, IsAutoIncrementing = true)]
        public long FilmId { get; set; }

        [Column("title", Size = 255)]
        public string Title { get; set; }

        [Column("description", Size = 65535)]
        public string Description { get; set; }

        [Column("release_year", Size = 0)]
        public string ReleaseYear { get; set; }

        [Column("language_id", Size = 3)]
        public byte LanguageId { get; set; }

        [Column("original_language_id", Size = 3)]
        public byte? OriginalLanguageId { get; set; }

        [Column("rental_duration", Size = 3)]
        public byte RentalDuration { get; set; }

        [Column("rental_rate", Size = 4)]
        public decimal RentalRate { get; set; }

        [Column("length", Size = 5)]
        public long? Length { get; set; }

        [Column("replacement_cost", Size = 5)]
        public decimal ReplacementCost { get; set; }

        [Column("rating", Size = 5)]
        public string Rating { get; set; }

        [Column("special_features", Size = 54)]
        public string SpecialFeatures { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime LastUpdate { get; set; }

        public Film()
        { }

        public Film(long filmId, string title, string description, string releaseYear, byte languageId, byte? originalLanguageId, byte rentalDuration, decimal rentalRate, long? length, decimal replacementCost, string rating, string specialFeatures, DateTime lastUpdate)
        {
            FilmId = filmId;
            Title = title;
            Description = description;
            ReleaseYear = releaseYear;
            LanguageId = languageId;
            OriginalLanguageId = originalLanguageId;
            RentalDuration = rentalDuration;
            RentalRate = rentalRate;
            Length = length;
            ReplacementCost = replacementCost;
            Rating = rating;
            SpecialFeatures = specialFeatures;
            LastUpdate = lastUpdate;
        }
    }
}
