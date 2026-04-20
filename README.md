# CSharpTest

A multi-target .NET solution demonstrating CSV-to-database import with separate executables for legacy (SQL CE on .NET Framework 4.7.2) and modern (SQL Server/SQLite on .NET 10) platforms.

## Architecture

```
CSharpTest.slnx
├── Import.Common    (netstandard2.0)  — Shared models, CSV reading, helpers
├── Import.Legacy    (net472)          — SQL CE import exe (EF Core 2.2.6)
├── Import.Modern    (net10.0)         — SQL Server/SQLite import exe (EF Core 10.0.7)
├── Logging          (netstandard2.0)  — Logging abstraction
├── Server           (net10.0)         — Server operation library
├── Retries          (net10.0)         — Retry pattern demo
└── Tests            (net10.0)         — xUnit test suite
```

### Design Principles

- **Import.Common** has no EF Core dependency — it targets `netstandard2.0` and is consumable by any .NET runtime
- Each exe owns its EF Core version independently — upgrading Modern doesn't affect Legacy
- Minimal duplication — only EF-coupled code (DbContext, Repository) is repeated per exe
- Dependency inversion via `ILogger`, `ICsvReader`, `IAppConfigurationLoader`, and `IDbContextOptionsFactory` for testability

## Projects

### Import.Common

Shared library containing framework-agnostic components.

- `Models/Employee.cs` — Entity with DataAnnotations (`[Table]`, `[Column]`, etc.)
- `Readers/CsvReader.cs` — Parses CSV using CsvHelper, logs and skips invalid rows
- `Readers/ICsvReader.cs` — Reader interface
- `Helpers/EmployeeCsvHelper.cs` — Loads embedded CSV resource, returns parsed employees
- `Configuration/AppConfiguration.cs` — Immutable DTO exposing `ProviderName` and `ConnectionString`
- `Configuration/AppConfigurationLoader.cs` — Loads `appsettings.json` into an `IAppConfiguration` (injectable via `IAppConfigurationLoader`)
- `Resources/Employees.csv` — Embedded CSV data

### Import.Legacy

Console application targeting .NET Framework 4.7.2 for SQL Server Compact databases.

- EF Core 2.2.6 with `EntityFrameworkCore.SqlServerCompact40`
- Single-file deployment via Costura.Fody
- `Providers/DbContextOptionsFactory.cs` — `IDbContextOptionsFactory` implementation that builds EF options from config
- `appsettings.json` — configures database provider and connection string

### Import.Modern

Console application targeting .NET 10 for SQL Server and SQLite databases.

- EF Core 10.0.7 with SqlServer and Sqlite providers
- `Providers/DbContextOptionsFactory.cs` — `IDbContextOptionsFactory` implementation that builds EF options from config
- `appsettings.json` — configures database provider (`MS SQL` or `SQLite`) and connection strings
- Single-file deployment via `PublishSingleFile` + `SelfContained`

### Retries

Console application demonstrating a retry policy pattern wrapping a server operation.

- Wraps `ServerInterface` with a configurable retry count
- Logs each attempt and surfaces the final result
- Single-file deployment via `PublishSingleFile` + `SelfContained`

### Server

Class library containing the server operation interface and implementation.

- `IServerInterface` — operation contract
- `ServerInterface` — implementation with logging
- `OperationResult` — result model with success/error states

### Logging

Shared class library providing a simple logging abstraction.

- `ILogger` — logging interface
- `ConsoleLogger` — writes messages to the console

### Tests

xUnit test project covering the solution.

- `CsvFileReaderTests` — CSV parsing (valid, empty, invalid rows, decimal handling)
- `OperationResultTests` — result model behavior
- `RetryPolicyTests` — retry logic with Moq
- `ServerInterfaceTests` — server operation behavior
- `AppConfigurationTests` — config loading, missing provider/connection string handling
- `DbContextOptionsFactoryTests` — factory produces correct EF options per provider

## Prerequisites

- .NET SDK 10.0+ (for building all projects)
- .NET Framework 4.7.2 targeting pack (for Import.Legacy)
- SQL Server Compact 4.0 runtime (for running Import.Legacy)

## Build

```
dotnet build CSharpTest.slnx
```

`dotnet clean` also removes runtime-generated database files (`.sdf`, `.db`) via `Directory.Build.targets`.

## Run

```
dotnet run --project Import.Legacy/Import.Legacy.csproj
dotnet run --project Import.Modern/Import.Modern.csproj
dotnet run --project Retries/Retries.csproj
```

## Test

```
dotnet test Tests/Tests.csproj
```

## Publish (single-file)

```
dotnet publish Import.Legacy/Import.Legacy.csproj -c Release
dotnet publish Import.Modern/Import.Modern.csproj -c Release -r win-x64
dotnet publish Retries/Retries.csproj -c Release -r win-x64
```