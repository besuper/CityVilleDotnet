using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace CityVilleDotnet.Test.Integration.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        await using var context = CreateDbContext();
        await context.Database.EnsureCreatedAsync();

        var testDataPath = Path.Combine(AppContext.BaseDirectory, "TestData");

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        StaticLogger.Configure(loggerFactory);
        GameSettingsManager.Instance.Initialize(loggerFactory.CreateLogger<GameSettingsManager>(), Path.Combine(testDataPath, "gameSettings.xml"));
        QuestSettingsManager.Instance.Initialize(loggerFactory.CreateLogger<QuestSettingsManager>(), Path.Combine(testDataPath, "questSettings.xml"));
    }

    public CityVilleDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<CityVilleDbContext>()
            .UseSqlServer(_container.GetConnectionString())
            .Options;

        return new CityVilleDbContext(options);
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>;