using GSqlQuery.MySql.Test.Data.Table;
using GSqlQuery.MySql.Test.Transform;
using GSqlQuery.Runner;
using System.Linq;

namespace GSqlQuery.MySql.Test
{
    public class MySqlDatabaseManagementEventsCustom : MySqlDatabaseManagementEvents
    {
        public override ITransformTo<T, TDbDataReader> GetTransformTo<T, TDbDataReader>(ClassOptions classOptions)
        {
            if (typeof(T) == typeof(Address))
            {
                return (ITransformTo<T, TDbDataReader>)new AddressTransform();
            }

            return base.GetTransformTo<T, TDbDataReader>(classOptions);
        }
    }
}
