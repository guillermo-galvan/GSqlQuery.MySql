namespace GSqlQuery.MySql
{
    public class MySqlConnectionOptions : ConnectionOptions<MySqlDatabaseConnection>
    {
        public MySqlConnectionOptions(string connectionString) :
            base(new MySqlFormats(), new MySqlDatabaseManagement(connectionString))
        { }

        public MySqlConnectionOptions(string connectionString, DatabaseManagementEvents events) :
            base(new MySqlFormats(), new MySqlDatabaseManagement(connectionString, events))
        { }

        public MySqlConnectionOptions(IFormats formats, MySqlDatabaseManagement mySqlDatabaseManagement) :
            base(formats, mySqlDatabaseManagement)
        { }
    }
}