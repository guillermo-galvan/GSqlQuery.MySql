using GSqlQuery.Runner;
using GSqlQuery.Runner.Transforms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Data.Common;

namespace GSqlQuery.MySql.Benchmark.Data.Transform
{
    internal abstract class TransformTo<T>(int numColumns) : TransformTo<T, MySqlDataReader>(numColumns) where T : class
    {
        protected T1 GetValue<T1>(PropertyOptionsInEntity column, MySqlDataReader reader)
        {
            if (column == null)
            {
                return default;
            }
            else if (!column.Ordinal.HasValue)
            {
                return (T1)column.DefaultValue;
            }
            else
            {
                var type = typeof(T1);

                if (type == typeof(MySqlGeometry))
                {
                    object result = reader.GetMySqlGeometry(column.Ordinal.Value);
                    return (T1)result;
                }

                var value = reader.GetValue(column.Ordinal.Value);
                return (T1)TransformTo.SwitchTypeValue(type, value);
            }
        }
    }
}
