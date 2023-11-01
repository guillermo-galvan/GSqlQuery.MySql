using GSqlQuery.Runner;
using GSqlQuery.Runner.Transforms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Data.Common;

namespace GSqlQuery.MySql.Benchmark.Data.Transform
{
    internal abstract class TransformTo<T>: Runner.Transforms.TransformTo<T> where T : class
    {
        public TransformTo(int numColumns) : base(numColumns)
        {
        }

        protected T1 GetValue<T1>(PropertyOptionsInEntity column, DbDataReader reader)
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
                    MySqlDataReader reader1 = reader as MySqlDataReader;
                    object result = reader1.GetMySqlGeometry(column.Ordinal.Value);
                    return (T1)result;
                }

                var value = reader.GetValue(column.Ordinal.Value);
                return (T1)TransformTo.SwitchTypeValue(type, value);
            }
        }
    }
}
