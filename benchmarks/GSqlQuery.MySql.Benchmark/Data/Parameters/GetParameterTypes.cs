using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace GSqlQuery.MySql.Benchmark.Data.Parameters
{
    internal abstract class GetParameterTypes<T> : IGetParameterTypes<T>
    {
        protected readonly Dictionary<string, MySqlDbType> _valuePairs;

        public GetParameterTypes()
        {
            _valuePairs = new Dictionary<string, MySqlDbType>();
        }

        public IDictionary<string, MySqlDbType> Types => _valuePairs;
    }
}
