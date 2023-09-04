using GSqlQuery.Runner;

namespace GSqlQuery.MySql
{
    public class MySqlConnectionOptions : ConnectionOptions<MySqlDatabaseConnection>
    {
        public MySqlConnectionOptions(string connectionString) :
            base(new MySqlStatements(), new MySqlDatabaseManagement(connectionString))
        { }

        public MySqlConnectionOptions(string connectionString, DatabaseManagementEvents events) :
            base(new MySqlStatements(), new MySqlDatabaseManagement(connectionString, events))
        { }

        public MySqlConnectionOptions(IStatements statements, MySqlDatabaseManagement mySqlDatabaseManagement) :
            base(statements, mySqlDatabaseManagement)
        { }
    }
}