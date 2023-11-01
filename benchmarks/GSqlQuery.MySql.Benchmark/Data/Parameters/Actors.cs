using GSqlQuery.MySql.Benchmark.Data.Table;
using MySql.Data.MySqlClient;

namespace GSqlQuery.MySql.Benchmark.Data.Parameters
{
    internal class Actors : GetParameterTypes<Actor>
    {
        public Actors() : base()
        {
            _valuePairs.Add(nameof(Actor.ActorId), MySqlDbType.Int32);
            _valuePairs.Add(nameof(Actor.FirstName), MySqlDbType.VarChar);
            _valuePairs.Add(nameof(Actor.LastName), MySqlDbType.VarChar);
            _valuePairs.Add(nameof(Actor.LastUpdate), MySqlDbType.Timestamp);
        }
    }
}
