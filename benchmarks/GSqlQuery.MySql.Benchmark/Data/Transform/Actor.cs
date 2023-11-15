using GSqlQuery.MySql.Benchmark.Data.Table;
using GSqlQuery.Runner;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace GSqlQuery.MySql.Benchmark.Data.Transform
{
    internal class Actors : TransformTo<Actor>
    {
        public Actors() : base(4)
        {
        }

        public override Actor Generate(IEnumerable<PropertyOptionsInEntity> columns, DbDataReader reader)
        {
            long actorId = GetValue<long>(columns.First(x => x.Property.PropertyInfo.Name == nameof(Actor.ActorId)), reader);
            string firstName = GetValue<string>(columns.First(x => x.Property.PropertyInfo.Name == nameof(Actor.FirstName)), reader);
            string lastName = GetValue<string>(columns.First(x => x.Property.PropertyInfo.Name == nameof(Actor.LastName)), reader);
            DateTime lastUpdate = GetValue<DateTime>(columns.First(x => x.Property.PropertyInfo.Name == nameof(Actor.LastUpdate)), reader);
            return new Actor(actorId, firstName, lastName, lastUpdate);
        }
    }
}
