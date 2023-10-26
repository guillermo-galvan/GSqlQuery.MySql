namespace GSqlQuery.MySql.Test.Data.Views
{
    [Table("sakila", "actor_info")]
    public class Actor_Info : Entity<Actor_Info>
    {
        [Column("actor_id", Size = 5)]
        public long ActorId { get; set; }

        [Column("first_name", Size = 45)]
        public string FirstName { get; set; }

        [Column("last_name", Size = 45)]
        public string LastName { get; set; }

        [Column("film_info", Size = 65535)]
        public string FilmInfo { get; set; }

        public Actor_Info()
        { }

        public Actor_Info(long actorId, string firstName, string lastName, string filmInfo)
        {
            ActorId = actorId;
            FirstName = firstName;
            LastName = lastName;
            FilmInfo = filmInfo;
        }
    }
}
