namespace Import.Common.Configuration
{
    public interface IAppConfiguration
    {
        string ConnectionString { get; }
        string ProviderName { get; }
    }
}
