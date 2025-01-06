using GSqlQuery.Runner.TypeHandles;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.TypeHandles
{
    internal class MySqlGeometryTypeHandler : TypeHandler<MySqlDataReader>
    {
        public override object GetValue(MySqlDataReader reader, int ordinal)
        {
            return reader.GetMySqlGeometry(ordinal);
        }

        public override Task<object> GetValueAsync(MySqlDataReader reader, int ordinal, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)reader.GetMySqlGeometry(ordinal));
        }

        protected override void SetDataType(IDataParameter dataParameter)
        {
            if(dataParameter is MySqlParameter mySqlParameter)
            {
                mySqlParameter.MySqlDbType = MySqlDbType.Geometry;
            }
            else
            {
                dataParameter.DbType = DbType.Object;
            }
        }
    }

    internal class MySqlGeometryNullableTypeHandler : MySqlGeometryTypeHandler
    {
        public override object GetValue(MySqlDataReader reader, int ordinal)
        {
            return  reader.IsDBNull(ordinal) ? null : reader.GetMySqlGeometry(ordinal);
        }

        public override async Task<object> GetValueAsync(MySqlDataReader reader, int ordinal, CancellationToken cancellationToken)
        {
            return await reader.IsDBNullAsync(ordinal, cancellationToken).ConfigureAwait(false) ? null : reader.GetMySqlGeometry(ordinal);
        }
    }
}
