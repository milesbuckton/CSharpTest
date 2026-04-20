using System;
using System.IO;
using Import.Common.Configuration;
using Import.Modern.Providers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests;

public class DbContextOptionsFactoryTests
{
    private static IAppConfiguration LoadConfig(string providerName, string connectionString)
    {
        string json = $$"""
            {
                "DatabaseProvider": "{{providerName}}",
                "ConnectionStrings": {
                    "{{providerName}}": "{{connectionString}}"
                }
            }
            """;
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, json);

        try
        {
            return new AppConfigurationLoader().Load(tempFile);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void Create_MsSql_ReturnsSqlServerOptions()
    {
        IAppConfiguration config = LoadConfig("MS SQL", "Server=localhost;Database=Test;Trusted_Connection=True;TrustServerCertificate=True;");

        var options = new DbContextOptionsFactory().Create(config);

        Assert.NotNull(options);
    }

    [Fact]
    public void Create_Sqlite_ReturnsSqliteOptions()
    {
        IAppConfiguration config = LoadConfig("SQLite", "Data Source=Test.db");

        var options = new DbContextOptionsFactory().Create(config);

        Assert.NotNull(options);
    }

    [Fact]
    public void Create_UnsupportedProvider_ThrowsArgumentException()
    {
        IAppConfiguration config = LoadConfig("Unknown", "Data Source=Test");

        Assert.Throws<ArgumentException>(() => new DbContextOptionsFactory().Create(config));
    }
}
