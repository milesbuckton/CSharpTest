using System;
using Import.Common.Configuration;
using Import.Legacy.Data;
using Microsoft.EntityFrameworkCore;

namespace Import.Legacy.Providers
{
    internal class DbContextOptionsFactory : IDbContextOptionsFactory
    {
        public DbContextOptions<EmployeeDbContext> Create(IAppConfiguration config) => config.ProviderName switch
        {
            "SQL CE" => new DbContextOptionsBuilder<EmployeeDbContext>().UseSqlCe(config.ConnectionString).Options,
            _ => throw new ArgumentException($"Unsupported provider: {config.ProviderName}")
        };
    }
}
