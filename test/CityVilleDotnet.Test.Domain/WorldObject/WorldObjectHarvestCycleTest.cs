using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.WorldObject;

[Collection("Domain")]
public class WorldObjectHarvestCycleTest(DomainFixture fixture)
{
    private const long OneHourMs = 3_600_000;

    [Fact]
    public void WorldObject_CanHarvest_ResidenceJustPlanted_ReturnsFalse()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        residence.CanHarvest().Should().BeFalse();
    }

    [Fact]
    public void WorldObject_CanHarvest_ResidenceAfterGrowTime_ReturnsTrue()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - OneHourMs);

        residence.CanHarvest().Should().BeTrue();
    }

    [Fact]
    public void WorldObject_Harvest_GrownResidence_ReplantsAtActionTime()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - OneHourMs);
        var actionTime = ServerUtils.GetCurrentTime();

        var (coinYield, _) = residence.Harvest(actionTime);

        coinYield.Should().Be(20);
        residence.State.Should().Be(WorldObjectState.Planted);
        residence.PlantTime.Should().Be(actionTime);
        residence.CanHarvest().Should().BeFalse();
    }

    [Fact]
    public void WorldObject_Harvest_GrownPlot_PlowsAndClearsContract()
    {
        var faker = new Faker();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Grown);

        var (coinYield, _) = plot.Harvest();

        coinYield.Should().Be(50);
        plot.State.Should().Be(WorldObjectState.Plowed);
        plot.ContractName.Should().BeNull();
        plot.CanHarvest().Should().BeFalse();
    }

    [Fact]
    public void WorldObject_OpenBusiness_Closed_OpensWithNoVisits()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed);

        business.OpenBusiness();

        business.State.Should().Be(WorldObjectState.Open);
        business.Visits.Should().Be(0);
        business.NeverOpened.Should().BeFalse();
        business.CanHarvest().Should().BeFalse();
    }

    [Fact]
    public void WorldObject_OpenBusiness_AlreadyOpen_ThrowsException()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Open);

        var act = () => business.OpenBusiness();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void WorldObject_UpdateVisits_BelowCommodityReq_StaysOpen()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed);
        business.OpenBusiness();

        business.UpdateVisits(14);

        business.State.Should().Be(WorldObjectState.Open);
        business.CanHarvest().Should().BeFalse();
    }

    [Fact]
    public void WorldObject_UpdateVisits_ReachesCommodityReq_BecomesClosedHarvestable()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed);
        business.OpenBusiness();

        business.UpdateVisits(15);

        business.State.Should().Be(WorldObjectState.ClosedHarvestable);
        business.CanHarvest().Should().BeTrue();
    }

    [Fact]
    public void WorldObject_UpdateVisits_AlreadyClosedHarvestable_IsIgnored()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed);
        business.OpenBusiness();
        business.UpdateVisits(15);

        business.UpdateVisits(5);

        business.State.Should().Be(WorldObjectState.ClosedHarvestable);
        business.Visits.Should().Be(15);
    }

    [Fact]
    public void WorldObject_Harvest_Business_ClosesAndResetsVisits()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed);
        business.OpenBusiness();
        business.UpdateVisits(15);

        business.Harvest();

        business.State.Should().Be(WorldObjectState.Closed);
        business.Visits.Should().Be(0);
        business.CanHarvest().Should().BeFalse();
    }

    [Fact]
    public void WorldObject_Plow_PlantedPlot_ClearsContract()
    {
        var faker = new Faker();
        var plot = faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        plot.Plow();

        plot.State.Should().Be(WorldObjectState.Plowed);
        plot.ContractName.Should().BeNull();
        plot.PlantTime.Should().BeNull();
    }
}
