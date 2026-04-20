using System.Collections.Generic;
using Import.Common.Models;

namespace Import.Common.Helpers
{
    public interface IEmployeeCsvHelper
    {
        List<Employee>? Load();
    }
}
