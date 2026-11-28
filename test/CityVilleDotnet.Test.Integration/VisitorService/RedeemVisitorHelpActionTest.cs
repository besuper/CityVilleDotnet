using AwesomeAssertions;
using CityVilleDotnet.Api.Services.VisitorService;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.VisitorHelpOrder;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.VisitorService;

// Replays what the client does in onVisitReplayAction when the owner redeems a neighbor help.
// The server derives the action from the target (MapResource::getVisitReplayEquivalentActionString)
[Collection("Database")]
public class RedeemVisitorHelpActionTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const string SenderId = "11835";
    private const int TargetId = 160;
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private RedeemVisitorHelpAction CreateHandler()
    {
        return new RedeemVisitorHelpAction(Context, NullLogger<RedeemVisitorHelpAction>.Instance, new ConfigurationBuilder().Build());
    }

    private static RedeemVisitorHelpActionRequest CreateRequest()
    {
        return new RedeemVisitorHelpActionRequest
        {
            SenderId = SenderId,
            WorldObjectId = TargetId
        };
    }

    private async Task<Player> SeedPlayer(WorldObject target, params int[] helpTargets)
    {
        var player = Faker.Player(world: Faker.World(objects: [target]));
        player.AddVisitorHelpOrder(Faker.VisitorHelpOrder(senderId: SenderId, helpTargets: helpTargets.Length > 0 ? helpTargets : [TargetId]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return player;
    }

    [Fact]
    public async Task RedeemVisitorHelpAction_WitheredPlot_RevivesPlotAndRemovesOrder()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(plot);

        time.Advance(TimeSpan.FromSeconds(300));

        var response = await CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        plot.State.Should().Be(WorldObjectState.Grown);
        plot.IsWithered().Should().BeFalse();
        plot.CanHarvest().Should().BeTrue();
        (await Context.Set<VisitorHelpOrder>().AnyAsync(x => x.SenderId == SenderId, TestContext.Current.CancellationToken)).Should().BeFalse();
    }

    [Fact]
    public async Task RedeemVisitorHelpAction_PlantedPlotUnderInstantLimit_BecomesGrown()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(plot);

        await CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        plot.State.Should().Be(WorldObjectState.Grown);
        plot.CanHarvest().Should().BeTrue();
    }

    // test_crop_slow: 4 * 82800s to grow, visit boost = 10% = 33120s
    [Fact]
    public async Task RedeemVisitorHelpAction_PlantedSlowPlot_BoostsGrowth()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var plantTime = ServerUtils.GetCurrentTime();
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "test_crop_slow", state: WorldObjectState.Planted, plantTime: plantTime, worldFlatId: TargetId);
        var player = await SeedPlayer(plot);

        await CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        plot.State.Should().Be(WorldObjectState.Planted);
        plot.PlantTime!.Value.Should().BeApproximately(plantTime - 33_120_000, 1);
    }

    [Fact]
    public async Task RedeemVisitorHelpAction_WitheredShip_RevivesShip()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var ship = Faker.WorldObject(itemName: "test_ship", className: BuildingClassType.Ship, contractName: "test_ship_contract", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(ship);

        time.Advance(TimeSpan.FromSeconds(300));

        await CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        ship.State.Should().Be(WorldObjectState.Grown);
        ship.IsWithered().Should().BeFalse();
        ship.CanHarvest().Should().BeTrue();
    }

    [Fact]
    public async Task RedeemVisitorHelpAction_ShipAwayUnderInstantLimit_BecomesGrown()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var ship = Faker.WorldObject(itemName: "test_ship", className: BuildingClassType.Ship, contractName: "test_ship_contract", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(ship);

        await CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        ship.State.Should().Be(WorldObjectState.Grown);
        ship.CanHarvest().Should().BeTrue();
    }

    // test_pier at (10, 10) has berth squares (9, 12) and (9, 9), test_ship at (6, 10) covers x 6..9 and y 10..12
    [Fact]
    public async Task RedeemVisitorHelpAction_LoadedShipBerthedAtPier_HarvestsAndCountsPierUpgradeAction()
    {
        var pier = Faker.WorldObject(itemName: "test_pier", className: BuildingClassType.Pier, x: 10, y: 10);
        var farPier = Faker.WorldObject(itemName: "test_pier", className: BuildingClassType.Pier, x: 30, y: 30);
        var ship = Faker.WorldObject(itemName: "test_ship", className: BuildingClassType.Ship, contractName: "test_ship_contract", state: WorldObjectState.Grown, x: 6, y: 10, worldFlatId: TargetId);
        var player = Faker.Player(world: Faker.World(objects: [pier, farPier, ship]));
        player.AddVisitorHelpOrder(Faker.VisitorHelpOrder(senderId: SenderId, helpTargets: [TargetId]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        await CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        var storedPier = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == pier.Id, TestContext.Current.CancellationToken);
        var storedFarPier = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == farPier.Id, TestContext.Current.CancellationToken);
        var storedShip = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == ship.Id, TestContext.Current.CancellationToken);
        storedPier.UpgradeActionCount.Should().Be(1);
        storedFarPier.UpgradeActionCount.Should().BeNull();
        storedShip.ContractName.Should().BeNull();
    }

    // test_bus_tour rolls test_bus_tour_table: 40 coins, without the visits multiplier of a real harvest
    [Fact]
    public async Task RedeemVisitorHelpAction_Business_RollsDoobersAndCountsUpgradeAction()
    {
        var business = Faker.WorldObject(itemName: "test_bus_tour", className: BuildingClassType.Business, state: WorldObjectState.Open, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(business);
        player.SetLevel(GameSettingsManager.Instance.GetSettings().BusinessUpgradesRequiredLevel);
        business.UpdateVisits(5);

        var goldBefore = player.Gold;

        await CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        player.Gold.Should().Be(goldBefore + 40);
        business.UpgradeActionCount.Should().Be(1);
        business.State.Should().Be(WorldObjectState.Open);
        business.Visits.Should().Be(5);
    }

    [Fact]
    public async Task RedeemVisitorHelpAction_BusinessBelowUpgradeLevel_RollsDoobersWithoutUpgradeAction()
    {
        var business = Faker.WorldObject(itemName: "test_bus_tour", className: BuildingClassType.Business, state: WorldObjectState.Open, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(business);
        player.SetLevel(GameSettingsManager.Instance.GetSettings().BusinessUpgradesRequiredLevel - 1);

        var goldBefore = player.Gold;

        await CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        player.Gold.Should().Be(goldBefore + 40);
        business.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public async Task RedeemVisitorHelpAction_OrderWithOtherTargets_OnlyRemovesRedeemedTarget()
    {
        var business = Faker.WorldObject(itemName: "test_bus_tour", className: BuildingClassType.Business, state: WorldObjectState.Open, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(business, TargetId, 161);

        await CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        var order = await Context.Set<VisitorHelpOrder>().SingleAsync(x => x.SenderId == SenderId, TestContext.Current.CancellationToken);
        order.HelpTargets.Should().Equal(161);
    }

    [Fact]
    public async Task RedeemVisitorHelpAction_NoOrderForTarget_Throws()
    {
        var business = Faker.WorldObject(itemName: "test_bus_tour", className: BuildingClassType.Business, state: WorldObjectState.Open, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(business, 161);
        var goldBefore = player.Gold;

        var act = () => CreateHandler().HandlePacket(CreateRequest(), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        player.Gold.Should().Be(goldBefore);
    }
}
