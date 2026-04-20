using System.Collections.Generic;
using Import.Common.Models;

namespace Import.Common.Data
{
    public interface IEmployeeDataContext
    {
        void AddEmployees(IEnumerable<Employee> employees);
        IReadOnlyList<Employee> GetEmployees();
        int SaveChanges();
    }
}
