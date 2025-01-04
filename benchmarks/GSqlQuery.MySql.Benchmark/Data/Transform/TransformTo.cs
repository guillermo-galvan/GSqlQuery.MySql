using GSqlQuery.Runner;
using MySql.Data.MySqlClient;

namespace GSqlQuery.MySql.Benchmark.Data.Transform
{
    internal abstract class TransformTo<T>(int numColumns) : TransformTo<T, MySqlDataReader>(numColumns) where T : class
    {
        
    }
}
