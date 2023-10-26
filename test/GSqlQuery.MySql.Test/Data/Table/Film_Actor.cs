using System;

namespace GSqlQuery.MySql.Test.Data.Table
{
    [Table("sakila", "film_actor")]
    public class Film_Actor : EntityExecute<Film_Actor>
    {
        [Column("actor_id", Size = 5, IsPrimaryKey = true)]
        public long ActorId { get; set; }

        [Column("film_id", Size = 5, IsPrimaryKey = true)]
        public long FilmId { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime LastUpdate { get; set; }

        public Film_Actor()
        { }

        public Film_Actor(long actorId, long filmId, DateTime lastUpdate)
        {
            ActorId = actorId;
            FilmId = filmId;
            LastUpdate = lastUpdate;
        }
    }
}
