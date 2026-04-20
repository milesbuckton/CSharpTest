namespace Import.Common.Configuration
{
    public interface IAppConfiguration
    {
        string ProviderName { get; }
        string ConnectionString { get; }
    }
}
