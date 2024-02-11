using GSqlQuery.MySql.Benchmark.Data.Table;
using GSqlQuery.Runner;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.Benchmark.Data.Transform
{
    internal class Actors : TransformTo<Actor>
    {
        public Actors() : base(4)
        {
        }

        public override Actor Generate(IEnumerable<PropertyOptionsInEntity> columns, MySqlDataReader reader)
        {
            long actorId = GetValue<long>(columns.FirstOrDefault(x => x.Property.PropertyInfo.Name == nameof(Actor.ActorId)), reader);
            string firstName = GetValue<string>(columns.FirstOrDefault(x => x.Property.PropertyInfo.Name == nameof(Actor.FirstName)), reader);
            string lastName = GetValue<string>(columns.FirstOrDefault(x => x.Property.PropertyInfo.Name == nameof(Actor.LastName)), reader);
            DateTime lastUpdate = GetValue<DateTime>(columns.FirstOrDefault(x => x.Property.PropertyInfo.Name == nameof(Actor.LastUpdate)), reader);
            return new Actor(actorId, firstName, lastName, lastUpdate);
        }

        public override Task<Actor> GenerateAsync(IEnumerable<PropertyOptionsInEntity> columns, MySqlDataReader reader)
        {
            return Task.FromResult(Generate(columns, reader));
        }
    }
}
