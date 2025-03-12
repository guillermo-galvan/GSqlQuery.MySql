using Xunit;

namespace GSqlQuery.MySql.Test
{
    [Collection("GlobalTestServer")]
    public class MySqlConnectionOptionsTest
    {
        [Fact]
        public void Create_MySqlConnectionOptions_With_ConnectionString()
        {
            var mySqlConnectionOptions = new MySqlConnectionOptions(GlobalFixture.CONNECTIONSTRING);
            Assert.NotNull(mySqlConnectionOptions);
        }

        [Fact]
        public void Create_MySqlConnectionOptions_With_ConnectionString_and_events()
        {
            var mySqlConnectionOptions = new MySqlConnectionOptions(GlobalFixture.CONNECTIONSTRING, new MySqlDatabaseManagementEvents());
            Assert.NotNull(mySqlConnectionOptions);
        }

        [Fact]
        public void Create_MySqlConnectionOptions_With_formats_and_MySqlDatabaseManagementt()
        {
            var mySqlConnectionOptions = new MySqlConnectionOptions(new MySqlFormats(), new MySqlDatabaseManagement(GlobalFixture.CONNECTIONSTRING));
            Assert.NotNull(mySqlConnectionOptions);
        }
    }
}