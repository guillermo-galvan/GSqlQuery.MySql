# DatabaseManagementEvents

Clase para manejar los eventos que genera la ejecución de las consultas

### Propiedades

|                               |                                                                                            |
|-------------------------------|--------------------------------------------------------------------------------------------|
| IsTraceActive                 | Determina si el rastro está activado                                                       |

### Métodos

|                    |                                                          |
|--------------------|----------------------------------------------------------|
| GetParameter<T>    | Recupera los parámetros para usar en la consulta         |
| WriteTrace         | Escribe en la salida estándar                            |
| GetTransformTo<T>  | Obtiene una implementación de la interfaz ITransformTo   |