using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class MySqlConnectionOptionsTest
    {
        [Fact]
        public void Create_MySqlConnectionOptions_With_ConnectionString()
        {
            var mySqlConnectionOptions = new MySqlConnectionOptions(Helper.GetConnectionString());
            Assert.NotNull(mySqlConnectionOptions);
        }

        [Fact]
        public void Create_MySqlConnectionOptions_With_ConnectionString_and_events()
        {
            var mySqlConnectionOptions = new MySqlConnectionOptions(Helper.GetConnectionString(), new MySqlDatabaseManagementEvents());
            Assert.NotNull(mySqlConnectionOptions);
        }

        [Fact]
        public void Create_MySqlConnectionOptions_With_formats_and_MySqlDatabaseManagementt()
        {
            var mySqlConnectionOptions = new MySqlConnectionOptions(new MySqlFormats(), new MySqlDatabaseManagement(Helper.GetConnectionString()));
            Assert.NotNull(mySqlConnectionOptions);
        }
    }
}