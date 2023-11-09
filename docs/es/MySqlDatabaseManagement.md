# MySqlDatabaseManagement

Clase para poder ejecutar la consulta en base de datos

### Propiedades

|                               |                                                                         |
|-------------------------------|-------------------------------------------------------------------------|
| Events                        | Obtener y poner los eventos                                             |
| ConnectionString              | Obtener cadena de conexión                                              |

### Métodos

|                               |                                                                                     |
|-------------------------------|-------------------------------------------------------------------------------------|
| GetConnection                 | Obtiene la conexión abierta a la base de datos                                      |
| ExecuteReader<T>              | Obtener los renglones de una consulta                                               |
| ExecuteNonQuery               | Obtener el número de filas afectadas                                                |
| ExecuteScalar                 | Recupera el valor de la primera columna y el primer reglón                          | 
| GetConnectionAsync            | Obtiene la conexión abierta a la base de datos                                      |
| ExecuteReaderAsync<T>         | Obtener los renglones de una consulta                                               |
| ExecuteNonQueryAsync          | Obtener el número de filas afectadas                                                |
| ExecuteScalarAsync            | Recupera el valor de la primera columna y el primer reglón                          | 