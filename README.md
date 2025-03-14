# GSqlQuery.MySql [![es](https://img.shields.io/badge/lang-es-red.svg)](./README.es.md) [![NuGet](https://img.shields.io/nuget/v/GSqlQuery.MySql.svg)](https://www.nuget.org/packages/GSqlQuery.MySql)

A library to run queries generated by [GSqlQuery](https://github.com/guillermo-galvan/GSqlQuery) on the MySql database for .NET.

## Get Started

GSqlQuery.MySql can be installed using the [Nuget packages](https://www.nuget.org/packages/GSqlQuery.MySql) or the `dotnet` CLI

```shell
dotnet add package GSqlQuery.MySql --version 3.0.0
```
[See our documentation](./docs/en/Config.md) for instructions on how to use the package.

## Example

```csharp
using GSqlQuery.MySql;

MySqlConnectionOptions connectionOptions = new MySqlConnectionOptions("<connectionString>");

IEnumerable<Actor> rows = EntityExecute<Actor>.Select(connectionOptions).Build().Execute();
```

## Contributors

GSqlQuery.MySql is actively maintained by [Guillermo Galván](https://github.com/guillermo-galvan). Contributions are welcome and can be submitted using pull request.

## License
Copyright (c) Guillermo Galván. All rights reserved.

Licensed under the [Apache-2.0 license](./LICENSE).