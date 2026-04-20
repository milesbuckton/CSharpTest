using System.Collections.Generic;
using Import.Common.Models;

namespace Import.Common.Data
{
    public interface IEmployeeRepository
    {
        void InsertEmployees(IEnumerable<Employee> employees);
    }
}
