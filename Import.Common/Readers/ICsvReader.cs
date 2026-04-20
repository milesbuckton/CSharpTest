using System.Collections.Generic;
using System.IO;
using Import.Common.Models;

namespace Import.Common.Readers
{
    internal interface ICsvReader
    {
        List<Employee> GetEmployees(Stream? stream);
    }
}
