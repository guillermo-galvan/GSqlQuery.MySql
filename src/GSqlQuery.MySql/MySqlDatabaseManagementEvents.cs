using GSqlQuery.MySql.TypeHandles;
using GSqlQuery.Runner;
using GSqlQuery.Runner.TypeHandles;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;

namespace GSqlQuery.MySql
{
    public class MySqlDatabaseManagementEvents : DatabaseManagementEvents
    {
        public static readonly TypeHandleCollection<MySqlDataReader> TypeHandleCollection = TypeHandleCollection<MySqlDataReader>.Instance;

        public MySqlDatabaseManagementEvents()
        {
            if(!TypeHandleCollection.ContainsKey(typeof(MySqlGeometry)))
            {
               TypeHandleCollection.Add(typeof(MySqlGeometry), new MySqlGeometryTypeHandler());
            }

            if (!TypeHandleCollection.ContainsKey(typeof(MySqlGeometry?)))
            {
                TypeHandleCollection.Add(typeof(MySqlGeometry?), new MySqlGeometryNullableTypeHandler());
            }
        }

        protected override ITypeHandler<TDbDataReader> GetTypeHandler<TDbDataReader>(Type property)
        {
            return (ITypeHandler<TDbDataReader>)TypeHandleCollection.GetTypeHandler(property);
        }
    }
}