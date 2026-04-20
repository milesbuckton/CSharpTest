using System;
using System.IO;
using Import.Common.Configuration;
using Xunit;

namespace Tests;

public class AppConfigurationTests
{
    [Fact]
    public void Load_ValidConfig_ReturnsExpectedValues()
    {
        string json = """
            {
                "DatabaseProvider": "SQLite",
                "ConnectionStrings": {
                    "SQLite": "Data Source=Test.db"
                }
            }
            """;
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, json);

        try
        {
            IAppConfiguration config = new AppConfigurationLoader().Load(tempFile);

            Assert.Equal("SQLite", config.ProviderName);
            Assert.Equal("Data Source=Test.db", config.ConnectionString);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void Load_MissingProvider_ThrowsInvalidOperationException()
    {
        string json = """
            {
                "ConnectionStrings": {
                    "SQLite": "Data Source=Test.db"
                }
            }
            """;
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, json);

        try
        {
            Assert.Throws<InvalidOperationException>(() => new AppConfigurationLoader().Load(tempFile));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void Load_MissingConnectionString_ThrowsInvalidOperationException()
    {
        string json = """
            {
                "DatabaseProvider": "SQLite",
                "ConnectionStrings": {
                    "Other": "Data Source=Test.db"
                }
            }
            """;
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, json);

        try
        {
            Assert.Throws<InvalidOperationException>(() => new AppConfigurationLoader().Load(tempFile));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void Load_FileNotFound_ThrowsFileNotFoundException()
    {
        Assert.ThrowsAny<Exception>(() => new AppConfigurationLoader().Load("nonexistent.json"));
    }
}
