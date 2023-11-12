# EntityExecute

With the entity class you can generate queries for models and classes.

### Methods

|         |                                         |
|---------|-----------------------------------------|
| Insert  | Method to generate the query insert.  |
| Update  | Method to generate the update query.  |

### Static methods

|         |                                         |
|---------|-----------------------------------------|
| Select  | Method to generate the Select query  |
| Insert  | Method to generate the Insert query  |
| Update  | Method to generate the Update query  |
| Delete  | Method to generate the Delete query  |


## Example

### Static methods

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

### Expanding the class

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