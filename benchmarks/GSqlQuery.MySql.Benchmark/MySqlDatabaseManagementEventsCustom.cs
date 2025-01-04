using GSqlQuery.MySql.Benchmark.Data.Parameters;
using GSqlQuery.MySql.Benchmark.Data.Table;
using GSqlQuery.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace GSqlQuery.MySql.Benchmark
{
    public class MySqlDatabaseManagementEventsCustom : MySqlDatabaseManagementEvents
    {
        private readonly ServiceProvider _serviceProvider;

        public MySqlDatabaseManagementEventsCustom(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override ITransformTo<T, TDbDataReader> GetTransformTo<T, TDbDataReader>(ClassOptions classOptions)
        {
            return typeof(Actor) == typeof(T) ? (ITransformTo<T, TDbDataReader>)new Data.Transform.Actors() : _serviceProvider.GetService<ITransformTo<T, TDbDataReader>>();
        }
    }
}
