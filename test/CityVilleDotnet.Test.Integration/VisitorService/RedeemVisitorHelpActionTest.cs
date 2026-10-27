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
// Business, Plot (planted/withered) send an empty action (MapResource::getVisitReplayEquivalentActionString)
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

    private static RedeemVisitorHelpActionRequest CreateRequest(BuildingClassType className, string itemName)
    {
        return new RedeemVisitorHelpActionRequest
        {
            SenderId = SenderId,
            WorldObjectId = TargetId,
            ClassName = className,
            ItemName = itemName,
            Action = ""
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

        var response = await CreateHandler().HandlePacket(CreateRequest(BuildingClassType.Plot, "plot_crop"), player.Id, TestContext.Current.CancellationToken);

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

        await CreateHandler().HandlePacket(CreateRequest(BuildingClassType.Plot, "plot_crop"), player.Id, TestContext.Current.CancellationToken);

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

        await CreateHandler().HandlePacket(CreateRequest(BuildingClassType.Plot, "plot_crop"), player.Id, TestContext.Current.CancellationToken);

        plot.State.Should().Be(WorldObjectState.Planted);
        plot.PlantTime!.Value.Should().BeApproximately(plantTime - 33_120_000, 1);
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

        await CreateHandler().HandlePacket(CreateRequest(BuildingClassType.Business, "test_bus_tour"), player.Id, TestContext.Current.CancellationToken);

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

        await CreateHandler().HandlePacket(CreateRequest(BuildingClassType.Business, "test_bus_tour"), player.Id, TestContext.Current.CancellationToken);

        player.Gold.Should().Be(goldBefore + 40);
        business.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public async Task RedeemVisitorHelpAction_OrderWithOtherTargets_OnlyRemovesRedeemedTarget()
    {
        var business = Faker.WorldObject(itemName: "test_bus_tour", className: BuildingClassType.Business, state: WorldObjectState.Open, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(business, TargetId, 161);

        await CreateHandler().HandlePacket(CreateRequest(BuildingClassType.Business, "test_bus_tour"), player.Id, TestContext.Current.CancellationToken);

        var order = await Context.Set<VisitorHelpOrder>().SingleAsync(x => x.SenderId == SenderId, TestContext.Current.CancellationToken);
        order.HelpTargets.Should().Equal(161);
    }

    [Fact]
    public async Task RedeemVisitorHelpAction_NoOrderForTarget_Throws()
    {
        var business = Faker.WorldObject(itemName: "test_bus_tour", className: BuildingClassType.Business, state: WorldObjectState.Open, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: TargetId);
        var player = await SeedPlayer(business, 161);
        var goldBefore = player.Gold;

        var act = () => CreateHandler().HandlePacket(CreateRequest(BuildingClassType.Business, "test_bus_tour"), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        player.Gold.Should().Be(goldBefore);
    }
}
