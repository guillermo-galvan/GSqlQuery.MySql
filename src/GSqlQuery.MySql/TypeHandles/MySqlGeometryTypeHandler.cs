using GSqlQuery.Runner.TypeHandles;
using MySql.Data.MySqlClient;
using System.Data;

namespace GSqlQuery.MySql.TypeHandles
{
    internal class MySqlGeometryTypeHandler : TypeHandler<MySqlDataReader>
    {
        public override object GetValue(MySqlDataReader reader, DataReaderPropertyDetail dataReaderPropertyDetail)
        {
            return reader.GetMySqlGeometry(dataReaderPropertyDetail.Ordinal.Value);
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
        public override object GetValue(MySqlDataReader reader, DataReaderPropertyDetail dataReaderPropertyDetail)
        {
            return  reader.IsDBNull(dataReaderPropertyDetail.Ordinal.Value) ? null : reader.GetMySqlGeometry(dataReaderPropertyDetail.Ordinal.Value);
        }
    }
}
