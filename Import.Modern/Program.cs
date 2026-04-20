using System;
using System.Collections.Generic;
using Import.Common.Configuration;
using Import.Common.Data;
using Import.Common.Helpers;
using Import.Common.Models;
using Import.Common.Readers;
using Import.Modern.Data;
using Import.Modern.Providers;
using Logging;
using Microsoft.EntityFrameworkCore;

namespace Import.Modern
{
    internal static class Program
    {
        private static void Main()
        {
            ILogger logger = new ConsoleLogger();

            try
            {
                ICsvReader csvReader = new CsvReader(logger);
                IEmployeeCsvHelper csvHelper = new EmployeeCsvHelper(csvReader, logger);
                IReadOnlyList<Employee>? employees = csvHelper.Load();
                if (employees == null)
                    return;

                IAppConfigurationLoader configLoader = new AppConfigurationLoader();
                IAppConfiguration config = configLoader.Load();

                IDbContextOptionsFactory optionsFactory = new DbContextOptionsFactory();
                DbContextOptions<EmployeeDbContext> options = optionsFactory.Create(config);

                logger.LogMessage($"Using database provider: {config.ProviderName}");

                using EmployeeDbContext context = new(options);
                context.Database.EnsureCreated();
                IEmployeeRepository repository = new EmployeeRepository(context, logger);
                repository.InsertEmployees(employees);

                foreach (Employee employee in repository.SelectEmployees())
                {
                    logger.LogMessage($"{employee.Id}: {employee.FirstName} {employee.LastName}, Born: {employee.BirthDate:yyyy-MM-dd}, Salary: {employee.CurrentSalary:C}");
                }
            }
            catch (Exception ex)
            {
                logger.LogMessage($"Unexpected error: {ex.Message}");
            }
        }
    }
}
