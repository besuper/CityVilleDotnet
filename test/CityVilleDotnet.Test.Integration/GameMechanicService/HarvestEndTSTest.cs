using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

// test_skyscraper: duration 3600, upgradeActionMechanic, energy 2, 50 coins
// test_corporation: no duration, harvestLoopConfig timerDuration 600
// test_fishinghole: duration 0.000695, cast to 0 by the client so no timer
[Collection("Database")]
public class HarvestEndTSTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task HarvestEndTS_NoTimer_HarvestsAndStartsTimer()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var skyscraper = Faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper, worldFlatId: 42);
        var world = Faker.World(objects: [skyscraper]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var energyBefore = player.Energy;
        var handler = new HarvestEndTS(Context, new ConfigurationBuilder().Build());

        var response = await handler.HandlePacket(new HarvestEndTSRequest { ObjectId = 42, GameMode = "GMPlay" }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Gold.Should().Be(goldBefore + 50);
        player.Energy.Should().Be(energyBefore - 2);
        skyscraper.HarvestEndTime.Should().Be(StartTime.UtcDateTime.AddSeconds(3600));
        skyscraper.UpgradeActionCount.Should().Be(1);
    }

    [Fact]
    public async Task HarvestEndTS_TimerNotFinished_ThrowsWithoutRewards()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var skyscraper = Faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper, worldFlatId: 42);
        var world = Faker.World(objects: [skyscraper]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new HarvestEndTS(Context, new ConfigurationBuilder().Build());
        var request = new HarvestEndTSRequest { ObjectId = 42, GameMode = "GMPlay" };

        await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        time.Advance(TimeSpan.FromSeconds(3000));

        var goldBefore = player.Gold;
        var energyBefore = player.Energy;

        var act = () => handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        player.Gold.Should().Be(goldBefore);
        player.Energy.Should().Be(energyBefore);
        skyscraper.UpgradeActionCount.Should().Be(1);
    }

    [Fact]
    public async Task HarvestEndTS_TimerFinished_RestartsTimer()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var skyscraper = Faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper, worldFlatId: 42);
        var world = Faker.World(objects: [skyscraper]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new HarvestEndTS(Context, new ConfigurationBuilder().Build());
        var request = new HarvestEndTSRequest { ObjectId = 42, GameMode = "GMPlay" };

        await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        time.Advance(TimeSpan.FromSeconds(3600));

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        skyscraper.HarvestEndTime.Should().Be(StartTime.UtcDateTime.AddSeconds(7200));
        skyscraper.UpgradeActionCount.Should().Be(2);
    }

    [Fact]
    public async Task HarvestEndTS_NoDurationAttribute_UsesHarvestLoopTimer()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var corporation = Faker.WorldObject(itemName: "test_corporation", className: BuildingClassType.Corporation, worldFlatId: 42);
        var world = Faker.World(objects: [corporation]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new HarvestEndTS(Context, new ConfigurationBuilder().Build());

        var response = await handler.HandlePacket(new HarvestEndTSRequest { ObjectId = 42, GameMode = "GMPlay" }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        corporation.HarvestEndTime.Should().Be(StartTime.UtcDateTime.AddSeconds(600));
        corporation.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public async Task HarvestEndTS_FractionalDuration_KeepsNoTimer()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var fishingHole = Faker.WorldObject(itemName: "test_fishinghole", className: BuildingClassType.MechanicMapResource, worldFlatId: 42);
        var world = Faker.World(objects: [fishingHole]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new HarvestEndTS(Context, new ConfigurationBuilder().Build());

        var response = await handler.HandlePacket(new HarvestEndTSRequest { ObjectId = 42, GameMode = "GMPlay" }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        fishingHole.HarvestEndTime.Should().BeNull();
    }

    [Fact]
    public async Task HarvestEndTS_NoMechanicInGameMode_Throws()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var skyscraper = Faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper, worldFlatId: 42);
        var world = Faker.World(objects: [skyscraper]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new HarvestEndTS(Context, new ConfigurationBuilder().Build());

        var act = () => handler.HandlePacket(new HarvestEndTSRequest { ObjectId = 42, GameMode = "GMVisit" }, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        skyscraper.HarvestEndTime.Should().BeNull();
    }
}
