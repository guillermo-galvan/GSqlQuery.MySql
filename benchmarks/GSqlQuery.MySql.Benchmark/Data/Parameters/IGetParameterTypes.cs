using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace GSqlQuery.MySql.Benchmark.Data.Parameters
{
    internal interface IGetParameterTypes
    {
        IDictionary<string, MySqlDbType> Types { get; }
    }

    internal interface IGetParameterTypes<T> : IGetParameterTypes
    {

    }
}
