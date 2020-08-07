# Rhyze.Data

Data access layer for the backend. Implements the CQRS pattern in favor of repositories or contexts.

-------

## Technology Stack
* C# 8.0
* MS SQL Server
* [FluentMigrator](https://fluentmigrator.github.io/)
* [RepoDB](https://repodb.net/)
* Project Dependencies
  * [Rhyze.Core](../Rhyze.Core/README.md)

## Project Documentation

Create commands and/or queries by making a class implement either `ICommandAsync` or `IQueryAsync<T>`. Execute these by passing them to `ExecuteAsync` on an `IDatabase` instance. Double dispatch has been implemented, so you can also execute them by calling `ExecuteAsync` on either a command or query, passing in an `IDbConnection` instance.

There is a static constructor in the provided `Database` class that runs the SQL Server bootstrapping required by RepoDB. To keep the project domains separate, models in the Core project aren't decorated with any kind of DAL attributes, but instead are mapped in the `ModelMapping` class, also initialized by the static constructor.

Migrations are handled by FluentMigrator. When you create one, decorate it with a `MigrationAttribute`, using the current date as a long in the following format:
`{year}_{month}_{day}_{no_of_migrations_made_that_day}`. i.e.: `[Migration(2020_08_10_02)]` would be the 2nd migration made on August 10th, 2020.