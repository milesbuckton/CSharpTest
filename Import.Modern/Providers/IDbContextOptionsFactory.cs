using Import.Common.Configuration;
using Import.Modern.Data;
using Microsoft.EntityFrameworkCore;

namespace Import.Modern.Providers
{
    internal interface IDbContextOptionsFactory
    {
        DbContextOptions<EmployeeDbContext> Create(IAppConfiguration config);
    }
}
