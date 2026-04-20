using Import.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Import.Modern.Data
{
    internal class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }
    }
}
