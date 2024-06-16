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

        public override IEnumerable<IDataParameter> GetParameter<T>(IEnumerable<ParameterDetail> parameters)
        {
            Queue<MySqlParameter> mySqlParameters = new();
            IGetParameterTypes<T> getParameters;
            try
            {
                getParameters = typeof(Actor) == typeof(T) ? (IGetParameterTypes<T>) new Actors() : _serviceProvider.GetService<IGetParameterTypes<T>>();
            }
            catch
            {
                getParameters = null;
            }

            if (getParameters == null)
            {
                throw new InvalidProgramException($"Interface to IGetParameters not found for type {typeof(T)}");
            }

            foreach (var param in parameters)
            {
                MySqlDbType mySqlDbType = getParameters.Types[param.PropertyOptions.PropertyInfo.Name];

                mySqlParameters.Enqueue(new MySqlParameter(param.Name, mySqlDbType) { Value = param.Value });
            }

            return mySqlParameters;
        }

        public override ITransformTo<T, TDbDataReader> GetTransformTo<T, TDbDataReader>(ClassOptions classOptions)
        {
            return typeof(Actor) == typeof(T) ? (ITransformTo<T, TDbDataReader>)new Data.Transform.Actors() : _serviceProvider.GetService<ITransformTo<T, TDbDataReader>>();
        }
    }
}
