namespace Import.Common.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public string ConnectionString { get; }
        public string ProviderName { get; }

        public AppConfiguration(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;
        }
    }
}
