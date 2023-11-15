# MySqlDatabaseManagement

Class to be able to execute the query in the database

### Properties

|                               |                                                                         |
|-------------------------------|-------------------------------------------------------------------------|
| Events                        | Get and set events                                            |
| ConnectionString              | Get connection string                                              |

### Methods

|                               |                                                                                     |
|-------------------------------|-------------------------------------------------------------------------------------|
| GetConnection                 | Gets the open connection to the database                                      |
| ExecuteReader<T>              | Get the rows of a query                                               |
| ExecuteNonQuery               | Get the number of affected rows                                                |
| ExecuteScalar                 | Retrieves the value of the first column and the first row                          | 
| GetConnectionAsync            | Gets the open connection to the database                                      |
| ExecuteReaderAsync<T>         | Get the rows of a query                                               |
| ExecuteNonQueryAsync          | Get the number of affected rows                                                |
| ExecuteScalarAsync            | Retrieves the value of the first column and the first row                          | 