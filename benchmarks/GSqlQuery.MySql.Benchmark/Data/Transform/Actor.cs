using GSqlQuery.Cache;
using GSqlQuery.MySql.Benchmark.Data.Table;
using GSqlQuery.Runner;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.Benchmark.Data.Transform
{
    internal class Actors : ITransformTo<Actor, MySqlDataReader>
    {
        private Actor _actor;

        public Actors()
        {
            _actor = new Actor();
        }

        private struct ActorsOrdinal
        {
            public ActorsOrdinal()
            {
            }
            public int ActorId { get; set; } = -1;
            public int FirstName { get; set; } = -1;
            public int LastName { get; set; } = -1;
            public int LastUpdate { get; set; } = -1;
        }

        private ActorsOrdinal GetOrdinal(IQuery<Actor> query, MySqlDataReader reader)
        {
            var result = new ActorsOrdinal();

            foreach (KeyValuePair<string, PropertyOptions> item in query.Columns)
            {
                switch (item.Value.PropertyInfo.Name)
                {
                    case nameof(Actor.ActorId):
                        result.ActorId = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Actor.FirstName):
                        result.FirstName = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Actor.LastName):
                        result.LastName = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Actor.LastUpdate):
                        result.LastUpdate = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public IEnumerable<Actor> Transform(PropertyOptionsCollection propertyOptions, IQuery<Actor> query, MySqlDataReader reader, DatabaseManagementEvents events)
        {
            List<Actor> result = [];

            ActorsOrdinal ordinals = GetOrdinal(query, reader);

            while (reader.Read())
            {
                long actorId = default;
                string firstName = default;
                string lastName = default;
                DateTime lastUpdate = default;

                foreach (KeyValuePair<string, PropertyOptions> item in query.Columns)
                {
                    switch (item.Value.PropertyInfo.Name)
                    {
                        case nameof(Actor.ActorId):
                            actorId = reader.IsDBNull(ordinals.ActorId) ? actorId : reader.GetInt64(ordinals.ActorId);
                            break;
                        case nameof(Actor.FirstName):
                            firstName = reader.IsDBNull(ordinals.FirstName) ? firstName : reader.GetString(ordinals.FirstName);
                            break;
                        case nameof(Actor.LastName):
                            lastName = reader.IsDBNull(ordinals.LastName) ? lastName : reader.GetString(ordinals.LastName);
                            break;
                        case nameof(Actor.LastUpdate):
                            lastUpdate = reader.IsDBNull(ordinals.LastUpdate) ? lastUpdate : reader.GetDateTime(ordinals.LastUpdate);
                            break;
                        default:
                            break;
                    }
                }

                result.Add(new Actor(actorId, firstName, lastName, lastUpdate));
            }

            return result;
        }

        public async Task<IEnumerable<Actor>> TransformAsync(PropertyOptionsCollection propertyOptions, IQuery<Actor> query, MySqlDataReader reader, DatabaseManagementEvents events, CancellationToken cancellationToken = default)
        {
            List<Actor> result = [];

            ActorsOrdinal ordinals = GetOrdinal(query, reader);

            while (await reader.ReadAsync(cancellationToken))
            {
                long actorId = default;
                string firstName = default;
                string lastName = default;
                DateTime lastUpdate = default;

                foreach (KeyValuePair<string, PropertyOptions> item in query.Columns)
                {
                    switch (item.Value.PropertyInfo.Name)
                    {
                        case nameof(Actor.ActorId):
                            actorId = await reader.IsDBNullAsync(ordinals.ActorId, cancellationToken) ? actorId : reader.GetInt64(ordinals.ActorId);
                            break;
                        case nameof(Actor.FirstName):
                            firstName = await reader.IsDBNullAsync(ordinals.FirstName, cancellationToken) ? firstName : reader.GetString(ordinals.FirstName);
                            break;
                        case nameof(Actor.LastName):
                            lastName = await reader.IsDBNullAsync(ordinals.LastName, cancellationToken) ? lastName : reader.GetString(ordinals.LastName);
                            break;
                        case nameof(Actor.LastUpdate):
                            lastUpdate = await reader.IsDBNullAsync(ordinals.LastUpdate, cancellationToken) ? lastUpdate : reader.GetDateTime(ordinals.LastUpdate);
                            break;
                        default:
                            break;
                    }
                }

                result.Add(new Actor(actorId, firstName, lastName, lastUpdate));
            }

            return result;
        }

        public void SetValue(PropertyOptions property, object value)
        {
            switch (property.PropertyInfo.Name)
            {
                case nameof(Actor.ActorId):
                    _actor.ActorId = (long)value;
                    break;
                case nameof(Actor.FirstName):
                    _actor.FirstName = (string)value;
                    break;
                case nameof(Actor.LastName):
                    _actor.LastName = (string)value;
                    break;
                case nameof(Actor.LastUpdate):
                    _actor.LastUpdate = (DateTime)value;
                    break;
                default:
                    break;
            }
        }

        public Actor GetEntity()
        {
            Actor result = _actor;
            _actor = new Actor();
            return result;
        }
    }
}
