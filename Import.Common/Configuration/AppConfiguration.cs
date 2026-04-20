namespace Import.Common.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public string ProviderName { get; }
        public string ConnectionString { get; }

        public AppConfiguration(string providerName, string connectionString)
        {
            ProviderName = providerName;
            ConnectionString = connectionString;
        }
    }
}
