using System.Collections.Generic;
using Import.Common.Data;
using Import.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Import.Modern.Data
{
    internal class EmployeeDbContext : DbContext, IEmployeeDataContext
    {
        public DbSet<Employee> Employees => Set<Employee>();

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        public void AddEmployees(IEnumerable<Employee> employees) => Employees.AddRange(employees);

        public IReadOnlyList<Employee> GetEmployees() => [.. Employees];
    }
}
