# Configuración

En esta sección encontrará una explicación de como poder utilizar el paquete.

## Conexion 
La configuracion mas simple que usted puede utilizar es crear una instancia de la clase [MySqlConnectionOptions](MySqlConnectionOptions.md) es pasandole como parametro en el contructor la cadena de conexion.

```csharp
MySqlConnectionOptions connectionOptions = new MySqlConnectionOptions("<connectionString>");
```
 
 ## Clase Entity Execute
Para poder simplificar un poco la escritura del código usted puede utilizar la clase [EntityExecute](EntityExecute.md).

```csharp
using GSqlQuery;

[Table("sakila", "city")]
public class City : EntityExecute<City>
{
    [Column("city_id", Size = 5, IsAutoIncrementing = true, IsPrimaryKey = true)]
    public long CityId { get; set; }

    [Column("city", Size = 50)]
    public string Name { get; set; }

    [Column("country_id", Size = 5)]
    public long CountryId { get; set; }

    [Column("last_update", Size = 19)]
    public DateTime LastUpdate { get; set; }

    public City()
    { }

    public City(long cityId, string name, long countryId, DateTime lastUpdate)
    {
        CityId = cityId;
        Name = name;
        CountryId = countryId;
        LastUpdate = lastUpdate;
    }
}
```

# Consultas

Usted puede crear las mismas consultas que se crean con [GSqlQuery](https://github.com/guillermo-galvan/GSqlQuery).

- [Insert](#insert)
- [Update](#update)
- [Select](#select)
- [Delete](#delete)
- [Count](#count)
- [Join](#join) 

Se agrega la consulta.

- [Limit](#limit).

## Insert

```csharp
using GSqlQuery;

City city = City.Insert(connectionOptions, new City
{
    CityId = 1,
    Name = "A Corua (La Corua)",
    CountryId = 87,
    LastUpdate = DateTime.Now
}).Build().Execute();

```

## Update

```csharp
using GSqlQuery;

int count = City.Update(connectionOptions, x => x.Name, "Abha")
                .Where()
                .Equal(x => x.CityId, 1)
                .Build().Execute();
```

## Select

```csharp
using GSqlQuery;

IEnumerable<City> rows = City.Select(connectionOptions)
                             .Where()
                             .Equal(x => x.CityId, 1)
                             .Build().Execute();
```

## Delete

```csharp
using GSqlQuery;

int count = City.Delete(connectionOptions)
                .Where()
                .Equal(x => x.CityId, 1)
                .Build().Execute();
```

## Count

```csharp
using GSqlQuery;

int count = City.Select(connectionOptions)
                .Count()
                .Where()
                .Equal(x => x.CityId, 1)
                .Build().Execute();
```

## Join

```csharp
using GSqlQuery;

IEnumerable<Join<City,Country>> rows = City.Select(connectionOptions)
                                           .InnerJoin<Country>()
                                           .Equal(x => x.Table1.CountryId, x => x.Table2.CountryId)
                                           .Where()
                                           .Equal(x => x.Table1.CityId, 1)
                                           .Build().Execute();
```
## Limit

```csharp
using GSqlQuery;

IEnumerable<City> rows = City.Select(connectionOptions)
                             .Where()
                             .Equal(x => x.CityId, 1)
                             .Limit(0,10)
                             .Build().Execute();
```

# Métodos Execute

Para poder ejecutar las consultas usted cuenta con dos métodos

- [Execute](#execute)
- [ExecuteAsync](#executeAsync)

# Execute

El método Execute realiza las consultas de forma sincrona y tiene una sobre carga para recibir la conexion que quiere utilizar.

```csharp
using GSqlQuery;

MySqlConnectionOptions connectionOptions = new MySqlConnectionOptions("<connectionString>");
IEnumerable<City> rows = City.Select(connectionOptions)
                             .Where()
                             .Equal(x => x.CityId, 1)
                             .Build().Execute();
```

```csharp
using GSqlQuery;

MySqlConnectionOptions connectionOptions = new MySqlConnectionOptions("<connectionString>");

using var connection = connectionOptions.DatabaseManagement.GetConnection();

IEnumerable<City> rows = City.Select(connectionOptions)
                             .Where()
                             .Equal(x => x.CityId, 1)
                             .Build().Execute(connection);

connection.Close();
```

# ExecuteAsync

El método ExecuteAsync realiza las consultas de forma asincrona y tiene una sobre carga para recibir la conexion que quiere utilizar.

```csharp
using GSqlQuery;

MySqlConnectionOptions connectionOptions = new MySqlConnectionOptions("<connectionString>");
IEnumerable<City> rows = await City.Select(connectionOptions)
                                   .Where()
                                   .Equal(x => x.CityId, 1)
                                   .Build().ExecuteAsync();
```

```csharp
using GSqlQuery;

 MySqlConnectionOptions connectionOptions = new MySqlConnectionOptions("<connectionString>");

 using var connection = await connectionOptions.DatabaseManagement.GetConnectionAsync();

 IEnumerable<City> rows = await City.Select(connectionOptions)
                                    .Where()
                                    .Equal(x => x.CityId, 1)
                                    .Build().ExecuteAsync(connection);

 await connection.CloseAsync();
```
