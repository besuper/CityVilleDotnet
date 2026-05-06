using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Settings;
using Microsoft.Extensions.Logging;

namespace CityVilleDotnet.Test.Domain.Fixtures;

public class DomainFixture
{
    public DomainFixture()
    {
        var testDataPath = Path.Combine(AppContext.BaseDirectory, "TestData");

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        StaticLogger.Configure(loggerFactory);
        GameSettingsManager.Instance.Initialize(loggerFactory.CreateLogger<GameSettingsManager>(), Path.Combine(testDataPath, "gameSettings.xml"));
        QuestSettingsManager.Instance.Initialize(loggerFactory.CreateLogger<QuestSettingsManager>(), Path.Combine(testDataPath, "questSettings.xml"));
    }
}

[CollectionDefinition("Domain")]
public class DomainCollection : ICollectionFixture<DomainFixture>;
