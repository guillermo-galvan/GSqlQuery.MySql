using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GSqlQuery.MySql
{
    public class MySqlDatabaseManagementEvents : DatabaseManagementEvents
    {
        public override IEnumerable<IDataParameter> GetParameter<T>(IEnumerable<ParameterDetail> parameters)
        {
            return parameters.Select(x => new MySqlParameter(x.Name, x.Value));
        }
    }
}