using System;
using System.Collections.Generic;
using Import.Common.Models;
using Logging;

namespace Import.Common.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IEmployeeDataContext _context;
        private readonly ILogger _logger;

        public EmployeeRepository(IEmployeeDataContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public void InsertEmployees(IEnumerable<Employee> employees)
        {
            try
            {
                _context.AddEmployees(employees);
                int totalRowsAffected = _context.SaveChanges();
                _logger.LogMessage($"{totalRowsAffected} row(s) inserted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogMessage($"Insert failed: {ex.Message}");
                throw;
            }
        }

        public IReadOnlyList<Employee> SelectEmployees()
        {
            try
            {
                IReadOnlyList<Employee> employees = _context.GetEmployees();
                _logger.LogMessage($"{employees.Count} employee(s) retrieved.");

                return employees;
            }
            catch (Exception ex)
            {
                _logger.LogMessage($"Select failed: {ex.Message}");
                throw;
            }
        }
    }
}
