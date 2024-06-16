using GSqlQuery.MySql.Test.Data.Table;
using GSqlQuery.MySql.Test.Transform;
using GSqlQuery.Runner;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GSqlQuery.MySql.Test
{
    public class MySqlDatabaseManagementEventsCustom : MySqlDatabaseManagementEvents
    {
        private MySqlParameter GetAddressParam(ParameterDetail parameterDetail)
        {
            if (parameterDetail.PropertyOptions.PropertyInfo.Name == nameof(Address.Location))
            {
                return new MySqlParameter(parameterDetail.Name, parameterDetail.Value) { MySqlDbType = MySqlDbType.Geometry};
            }

            return new MySqlParameter(parameterDetail.Name, parameterDetail.Value);
        }

        public override IEnumerable<IDataParameter> GetParameter<T>(IEnumerable<ParameterDetail> parameters)
        {
            if(typeof(T) == typeof(Address))
            {
                return parameters.Select(GetAddressParam);
            }
            
            return parameters.Select(x => new MySqlParameter(x.Name, x.Value));
        }

        public override ITransformTo<T, TDbDataReader> GetTransformTo<T, TDbDataReader>(ClassOptions classOptions)
        {
            if (typeof(T) == typeof(Address))
            {
                return (ITransformTo<T, TDbDataReader>)new AddressTransform(classOptions.PropertyOptions.Count());
            }

            return base.GetTransformTo<T, TDbDataReader>(classOptions);
        }
    }
}
