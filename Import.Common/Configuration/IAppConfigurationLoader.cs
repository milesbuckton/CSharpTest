namespace Import.Common.Configuration
{
    public interface IAppConfigurationLoader
    {
        IAppConfiguration Load(string jsonFile = "appsettings.json");
    }
}
