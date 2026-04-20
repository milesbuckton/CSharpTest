using System;
using Import.Common.Configuration;
using Import.Modern.Data;
using Microsoft.EntityFrameworkCore;

namespace Import.Modern.Providers
{
    internal class DbContextOptionsFactory : IDbContextOptionsFactory
    {
        public DbContextOptions<EmployeeDbContext> Create(IAppConfiguration config) => config.ProviderName switch
        {
            "MS SQL" => new DbContextOptionsBuilder<EmployeeDbContext>().UseSqlServer(config.ConnectionString).Options,
            "SQLite" => new DbContextOptionsBuilder<EmployeeDbContext>().UseSqlite(config.ConnectionString).Options,
            _ => throw new ArgumentException($"Unsupported provider: {config.ProviderName}")
        };
    }
}
