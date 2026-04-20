using Import.Common.Configuration;
using Import.Legacy.Data;
using Microsoft.EntityFrameworkCore;

namespace Import.Legacy.Providers
{
    internal interface IDbContextOptionsFactory
    {
        DbContextOptions<EmployeeDbContext> Create(IAppConfiguration config);
    }
}
