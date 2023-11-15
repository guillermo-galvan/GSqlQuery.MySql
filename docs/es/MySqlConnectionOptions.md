# MySqlConnectionOptions

Clase para poder configurar la conexión a base de datos

### Constructores

|                                                                                            |                                                                                        |
|--------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------|
| MySqlConnectionOptions(string connectionString)                                            | Inicializar una instancia con la cadena de conexion que se pasa como parametro         |
| MySqlConnectionOptions(string connectionString, DatabaseManagementEvents events)           | Inicializar una instancia con la cadena de conexion que se pasa como parametro y una clase derivada de [DatabaseManagementEvents](DatabaseManagementEvents.md) |
| MySqlConnectionOptions(IFormats formats, MySqlDatabaseManagement mySqlDatabaseManagement)  | Inicializar una instancia con el formato a utlizar y una intancia de [MySqlDatabaseManagement](MySqlDatabaseManagement.md)   |

### Propiedades

|                               |                                                                                            |
|-------------------------------|--------------------------------------------------------------------------------------------|
| Format                        | Da el formato al nombre de la columna y tabla                                              |
| DatabaseManagement            | Administrador de la conexión a base de datos                                               |

```csharp
using GSqlQuery.MySql;

MySqlConnectionOptions mySqlConnectionOptions = new MySqlConnectionOptions("<connectionString>");

MySqlConnectionOptions mySqlConnectionOptions = new MySqlConnectionOptions("<connectionString>", new MySqlDatabaseManagementEvents());

MySqlConnectionOptions mySqlConnectionOptions = new MySqlConnectionOptions(new MySqlFormats(), new MySqlDatabaseManagement("<connectionString>"));

```