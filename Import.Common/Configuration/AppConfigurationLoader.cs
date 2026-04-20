using System;
using Microsoft.Extensions.Configuration;

namespace Import.Common.Configuration
{
    public class AppConfigurationLoader : IAppConfigurationLoader
    {
        public IAppConfiguration Load(string jsonFile = "appsettings.json")
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile(jsonFile).Build();

            string providerName = configuration["DatabaseProvider"]
                ?? throw new InvalidOperationException("DatabaseProvider is not configured in appsettings.json.");

            string connectionString = configuration.GetConnectionString(providerName)
                ?? throw new InvalidOperationException($"Connection string not found for provider: {providerName}");

            return new AppConfiguration(providerName, connectionString);
        }
    }
}
