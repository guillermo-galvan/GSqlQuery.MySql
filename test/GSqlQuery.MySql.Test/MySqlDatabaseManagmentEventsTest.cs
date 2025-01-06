using GSqlQuery.MySql.Test.Data.Table;
using MySql.Data.MySqlClient;
using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class MySqlDatabaseManagementEventsTest
    {
        private readonly MySqlConnectionOptions _connectionOptions;

        public MySqlDatabaseManagementEventsTest()
        {
            _connectionOptions = new MySqlConnectionOptions(Helper.GetConnectionString(), new MySqlDatabaseManagementEventsCustom());
        }

        [Fact]
        public void GetTransformTo_ReturnsNotNull_ForAddressType()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Address));
            var result = _connectionOptions.DatabaseManagement.Events.GetTransformTo<Address, MySqlDataReader>(classOptions);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTransformTo_ReturnsNotNull_ForFilmType()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Film));
            var result = _connectionOptions.DatabaseManagement.Events.GetTransformTo<Film, MySqlDataReader>(classOptions);
            Assert.NotNull(result);
        }
    }
}