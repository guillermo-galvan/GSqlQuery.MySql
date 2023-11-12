# EntityExecute

Con la clase entity usted puede generar las consultas de los modelos y clases.

### Métodos

|         |                                         |
|---------|-----------------------------------------|
| Insert  | Método para generar la consulta insert  |
| Update  | Método para generar la consulta update  |

### Métodos estáticos

|         |                                         |
|---------|-----------------------------------------|
| Select  | Método para generar la consulta select  |
| Insert  | Método para generar la consulta insert  |
| Update  | Método para generar la consulta update  |
| Delete  | Método para generar la consulta delete  |


## Ejemplos

### Métodos estáticos

```csharp
using GSqlQuery;

[Table("sakila", "city")]
public class City
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

MySqlConnectionOptions connectionOptions = new MySqlConnectionOptions("<connectionString>");
var rows = EntityExecute<City>.Select(connectionOptions).Build().Execute();
```

### Extendiendo de la clase

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

MySqlConnectionOptions connectionOptions = new MySqlConnectionOptions("<connectionString>");
var rows = City.Select(connectionOptions).Build().Execute();
```