using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GSqlQuery.MySql
{
    public class MySqlDatabaseManagementEvents : DatabaseManagementEvents
    {
        public override Func<Type, IEnumerable<ParameterDetail>, IEnumerable<IDataParameter>> OnGetParameter { get; set; } = (type, parametersDetail) =>
        {
            return parametersDetail.Select(x => new MySqlParameter(x.Name, x.Value));
        };
    }
}