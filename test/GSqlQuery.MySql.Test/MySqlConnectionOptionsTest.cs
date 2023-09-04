using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class MySqlConnectionOptionsTest
    {
        [Fact]
        public void Create_MySqlConnectionOptions_With_ConnectionString()
        {
            var SqliteConnectionOptions = new MySqlConnectionOptions(Helper.ConnectionString);
            Assert.NotNull(SqliteConnectionOptions);
        }

        [Fact]
        public void Create_MySqlConnectionOptions_With_ConnectionString_and_events()
        {
            var SqliteConnectionOptions = new MySqlConnectionOptions(Helper.ConnectionString, new MySqlDatabaseManagementEvents());
            Assert.NotNull(SqliteConnectionOptions);
        }

        [Fact]
        public void Create_MySqlConnectionOptions_With_statements_and_sqlServerDatabaseManagement()
        {
            var SqliteConnectionOptions = new MySqlConnectionOptions(new MySqlStatements(), new MySqlDatabaseManagement(Helper.ConnectionString));
            Assert.NotNull(SqliteConnectionOptions);
        }
    }
}