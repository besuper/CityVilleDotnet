using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.WorldObject;

// grown after growTime * inGameDaySeconds * growMultiplier, withered after growTime * inGameDaySeconds * witherMultiplier
// plot_strawberries: 0.001 * 82800s = 82.8s to grow, 248.4s to wither. res_cottage3: 0.005 * 82800s = 414s to grow
[Collection("Domain")]
public class WorldObjectGrowthTest
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void WorldObject_CanHarvest_PlotBeforeGrowTime_ReturnsFalse()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        time.Advance(TimeSpan.FromSeconds(82));

        plot.CanHarvest().Should().BeFalse();
    }

    [Fact]
    public void WorldObject_CanHarvest_PlotAfterGrowTime_ReturnsTrue()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        time.Advance(TimeSpan.FromSeconds(83));

        plot.CanHarvest().Should().BeTrue();
    }

    [Fact]
    public void WorldObject_IsWithered_GrownPlotBeforeWitherTime_ReturnsFalse()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        time.Advance(TimeSpan.FromSeconds(248));

        plot.IsWithered().Should().BeFalse();
        plot.CanHarvest().Should().BeTrue();
    }

    [Fact]
    public void WorldObject_IsWithered_PlotAfterWitherTime_ReturnsTrue()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        time.Advance(TimeSpan.FromSeconds(249));

        plot.IsWithered().Should().BeTrue();
    }

    [Fact]
    public void WorldObject_SetFullGrown_WitheredPlot_BecomesHarvestable()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        time.Advance(TimeSpan.FromSeconds(300));
        plot.SetFullGrown();

        plot.State.Should().Be(WorldObjectState.Grown);
        plot.PlantTime!.Value.Should().BeApproximately(ServerUtils.GetCurrentTime() - 82_800, 1);
        plot.IsWithered().Should().BeFalse();
        plot.CanHarvest().Should().BeTrue();
    }

    // instant limit: boostGrowInstantHourLimit / 23 * dailyCollectCycleTimeSeconds = 1 / 23 * 86400s ≈ 3756.5s
    [Fact]
    public void WorldObject_BoostPlot_GrowTimeUnderInstantLimit_BecomesGrown()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        plot.BoostPlot();

        plot.State.Should().Be(WorldObjectState.Grown);
        plot.CanHarvest().Should().BeTrue();
    }

    // test_crop_slow: 4 * 82800s = 331200s to grow, visit boost = 10% = 33120s
    [Fact]
    public void WorldObject_BoostPlot_GrowTimeOverInstantLimit_RemovesVisitBoostFromPlantTime()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var plantTime = ServerUtils.GetCurrentTime();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "test_crop_slow", state: WorldObjectState.Planted, plantTime: plantTime);

        plot.BoostPlot();

        plot.State.Should().Be(WorldObjectState.Planted);
        plot.PlantTime!.Value.Should().BeApproximately(plantTime - 33_120_000, 1);
        plot.CanHarvest().Should().BeFalse();
    }

    [Fact]
    public void WorldObject_BoostPlot_GrowTimeLeftUnderVisitBoost_BecomesGrown()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "test_crop_slow", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        time.Advance(TimeSpan.FromSeconds(331_200 - 33_119));
        plot.BoostPlot();

        plot.State.Should().Be(WorldObjectState.Grown);
        plot.CanHarvest().Should().BeTrue();
    }

    [Fact]
    public void WorldObject_BoostPlot_NotAPlot_Throws()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_tour", className: BuildingClassType.Business, state: WorldObjectState.Open, plantTime: ServerUtils.GetCurrentTime());

        var act = () => business.BoostPlot();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void WorldObject_Harvest_Residence_RegrowsAfterGrowTime()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - 500_000);

        residence.Harvest();

        time.Advance(TimeSpan.FromSeconds(413));

        residence.CanHarvest().Should().BeFalse();

        time.Advance(TimeSpan.FromSeconds(1));

        residence.CanHarvest().Should().BeTrue();
    }
}
