# MySqlConnectionOptions

Class to configure the connection to the database.

### Constructors

|                                                                                            |                                                                                        |
|--------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------|
| MySqlConnectionOptions(string connectionString)                                            | Initialize an instance with the connection string passed as a parameter         |
| MySqlConnectionOptions(string connectionString, DatabaseManagementEvents events)           | Initialize an instance with the connection string passed as a parameter and a class derived from [DatabaseManagementEvents](DatabaseManagementEvents.md) |
| MySqlConnectionOptions(IFormats formats, MySqlDatabaseManagement mySqlDatabaseManagement)  | Initialize an instance with the format to use and an instance of [MySqlDatabaseManagement](MySqlDatabaseManagement.md)   |

### Properties

|                               |                                                                                            |
|-------------------------------|--------------------------------------------------------------------------------------------|
| Format                        | Formats the column and table name                                                          |
| DatabaseManagement            | Database Connection Manager                                                                |

```csharp
using GSqlQuery.MySql;

MySqlConnectionOptions mySqlConnectionOptions = new MySqlConnectionOptions("<connectionString>");

MySqlConnectionOptions mySqlConnectionOptions = new MySqlConnectionOptions("<connectionString>", new MySqlDatabaseManagementEvents());

MySqlConnectionOptions mySqlConnectionOptions = new MySqlConnectionOptions(new MySqlFormats(), new MySqlDatabaseManagement("<connectionString>"));

```