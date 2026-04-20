using System;
using System.Collections.Generic;
using Import.Common.Data;
using Import.Common.Models;
using Logging;

namespace Import.Legacy.Data
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        private readonly ILogger _logger;

        public EmployeeRepository(EmployeeDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public void InsertEmployees(IEnumerable<Employee> employees)
        {
            try
            {
                _context.Employees.AddRange(employees);
                int totalRowsAffected = _context.SaveChanges();
                _logger.LogMessage($"{totalRowsAffected} row(s) inserted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogMessage($"Insert failed: {ex.Message}");
                throw;
            }
        }
    }
}
