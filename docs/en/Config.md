# Setting

In this section you will find an explanation of how to use the package.

## Connection

The simplest configuration you can use is to create an instance of the class [MySqlConnectionOptions](MySqlConnectionOptions.md) by passing the connection string as a parameter in the constructor.

```csharp
MySqlConnectionOptions connectionOptions = new MySqlConnectionOptions("<connectionString>");
```
 ## Entity  Execute Class
To simplify writing the code a little you can use the class [EntityExecute](EntityExecute.md).

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

# Queries

You can create the same queries that are created with [GSqlQuery](https://github.com/guillermo-galvan/GSqlQuery).

- [Insert](#insert)
- [Update](#update)
- [Select](#select)
- [Delete](#delete)
- [Count](#count)
- [Join](#join) 

The query is added.

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

# Methods 

In order to execute the queries you have two methods

- [Execute](#execute)
- [ExecuteAsync](#executeAsync)

# Execute 

The Execute method performs queries synchronously and has an overhead to receive the connection it wants to use.

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

The ExecuteAsync method queries asynchronously and has an overhead to receive the connection you want to use.

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
