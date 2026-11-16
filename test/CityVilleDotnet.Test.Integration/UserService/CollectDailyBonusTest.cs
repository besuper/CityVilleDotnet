using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.UserService;

// See dailyBonus and test_daily_* in TestData/gameSettings.xml
[Collection("Database")]
public class CollectDailyBonusTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private static FakeTimeProvider UseFakeTime()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        return time;
    }

    private CollectDailyBonus CreateHandler(int variant = 0)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["experiments:0:name"] = "test_dailybonus",
                ["experiments:0:variant"] = variant.ToString()
            })
            .Build();

        return new CollectDailyBonus(Context, configuration);
    }

    private async Task<Player> SeedPlayer(Player player)
    {
        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();
        return player;
    }

    [Fact]
    public async Task CollectDailyBonus_NewPlayer_GrantsDayOneAndPersistsStreak()
    {
        UseFakeTime();
        var player = await SeedPlayer(Faker.Player());
        var energyBefore = player.Energy;

        var response = await CreateHandler().HandlePacket([], player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        response["data"].Should().BeOfType<ASObject>().Which.Should().NotContainKey("randomReward");

        Context.ChangeTracker.Clear();
        var saved = await Context.Set<Player>().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);

        saved.Energy.Should().Be(energyBefore + 3);
        saved.DailyBonusDay.Should().Be(1);
        saved.LastDailyBonusDate.Should().Be(new DateOnly(2026, 1, 15));
    }

    [Fact]
    public async Task CollectDailyBonus_LastDayItem_ReturnsRandomRewardAndAddsItem()
    {
        var time = UseFakeTime();
        var days = GameSettingsManager.Instance.GetDailyBonusDays(new Dictionary<string, int>());
        var player = Faker.Player();
        player.CollectDailyBonus(days);
        time.Advance(TimeSpan.FromDays(1));
        player.CollectDailyBonus(days);
        time.Advance(TimeSpan.FromDays(1));
        await SeedPlayer(player);

        var response = await CreateHandler().HandlePacket([], player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var data = response["data"] as ASObject;
        data.Should().NotBeNull();
        data["randomReward"].Should().Be("test_daily_surprise");

        Context.ChangeTracker.Clear();
        var saved = await Context.Set<Player>().Include(x => x.InventoryItems).FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);

        saved.CountInventoryItem("test_daily_surprise").Should().Be(1);
        saved.DailyBonusDay.Should().Be(0);
    }

    [Fact]
    public async Task CollectDailyBonus_ExperimentVariant_UsesVariantTable()
    {
        UseFakeTime();
        var player = await SeedPlayer(Faker.Player());
        var goodsBefore = player.Goods;

        var response = await CreateHandler(variant: 1).HandlePacket([], player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var saved = await Context.Set<Player>().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);

        saved.Goods.Should().Be(goodsBefore + 25);
    }

    [Fact]
    public async Task CollectDailyBonus_XpReward_AddsXpAndLevelsUp()
    {
        var time = UseFakeTime();
        var player = Faker.Player();
        player.CollectDailyBonus(GameSettingsManager.Instance.GetDailyBonusDays(new Dictionary<string, int> { ["test_dailybonus"] = 1 }));
        time.Advance(TimeSpan.FromDays(1));
        await SeedPlayer(player);

        var response = await CreateHandler(variant: 1).HandlePacket([], player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var saved = await Context.Set<Player>().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);

        saved.Xp.Should().Be(10);
        saved.Level.Should().Be(2);
    }

    [Fact]
    public async Task CollectDailyBonus_AlreadyCollectedToday_ThrowsInvalidState()
    {
        UseFakeTime();
        var player = Faker.Player();
        player.CollectDailyBonus(GameSettingsManager.Instance.GetDailyBonusDays(new Dictionary<string, int>()));
        await SeedPlayer(player);

        var act = () => CreateHandler().HandlePacket([], player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidState);
    }
}
