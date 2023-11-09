using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class MySqlConnectionOptionsTest
    {
        [Fact]
        public void Create_MySqlConnectionOptions_With_ConnectionString()
        {
            var mySqlConnectionOptions = new MySqlConnectionOptions(Helper.ConnectionString);
            Assert.NotNull(mySqlConnectionOptions);
        }

        [Fact]
        public void Create_MySqlConnectionOptions_With_ConnectionString_and_events()
        {
            var mySqlConnectionOptions = new MySqlConnectionOptions(Helper.ConnectionString, new MySqlDatabaseManagementEvents());
            Assert.NotNull(mySqlConnectionOptions);
        }

        [Fact]
        public void Create_MySqlConnectionOptions_With_formats_and_sqlServerDatabaseManagement()
        {
            var mySqlConnectionOptions = new MySqlConnectionOptions(new MySqlFormats(), new MySqlDatabaseManagement(Helper.ConnectionString));
            Assert.NotNull(mySqlConnectionOptions);
        }
    }
}