using System;
using System.Collections.Generic;
using Import.Common.Configuration;
using Import.Common.Data;
using Import.Common.Helpers;
using Import.Common.Models;
using Import.Common.Readers;
using Import.Legacy.Data;
using Import.Legacy.Providers;
using Logging;
using Microsoft.EntityFrameworkCore;

namespace Import.Legacy
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
                List<Employee>? employees = csvHelper.Load();
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
            }
            catch (Exception ex)
            {
                logger.LogMessage($"Unexpected error: {ex.Message}");
            }
        }
    }
}
