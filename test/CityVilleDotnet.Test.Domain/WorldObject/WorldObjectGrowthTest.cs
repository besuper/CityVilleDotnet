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
public class WorldObjectGrowthTest(DomainFixture fixture)
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
