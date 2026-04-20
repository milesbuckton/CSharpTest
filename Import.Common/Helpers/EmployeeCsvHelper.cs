using System.Collections.Generic;
using System.IO;
using Import.Common.Models;
using Import.Common.Readers;
using Logging;

namespace Import.Common.Helpers
{
    public class EmployeeCsvHelper : IEmployeeCsvHelper
    {
        private readonly ICsvReader _csvReader;
        private readonly ILogger _logger;

        internal EmployeeCsvHelper(ICsvReader csvReader, ILogger logger)
        {
            _csvReader = csvReader;
            _logger = logger;
        }

        public List<Employee>? Load()
        {
            Stream? csvStream = typeof(EmployeeCsvHelper).Assembly
                .GetManifestResourceStream("Import.Common.Resources.Employees.csv");

            if (csvStream == null)
            {
                _logger.LogMessage("CSV resource not found.");
                return null;
            }

            List<Employee> employees = _csvReader.GetEmployees(csvStream);

            if (employees.Count == 0)
            {
                _logger.LogMessage("CSV file contains no employee data.");
            }

            return employees;
        }
    }
}
