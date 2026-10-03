using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.FranchiseLocation;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.WorldObject;

[Collection("Domain")]
public class WorldObjectUpgradeActionTest
{
    private static int RequiredLevel => GameSettingsManager.Instance.GetSettings().BusinessUpgradesRequiredLevel;

    [Fact]
    public void WorldObject_OpenBusiness_UpgradableAtRequiredLevel_CountsUpgradeAction()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed);

        business.OpenBusiness(RequiredLevel);

        business.UpgradeActionCount.Should().Be(1);
    }

    [Fact]
    public void WorldObject_OpenBusiness_BelowRequiredLevel_DoesNotCountUpgradeAction()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed);

        business.OpenBusiness(RequiredLevel - 1);

        business.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public void WorldObject_OpenBusiness_NotUpgradable_DoesNotCountUpgradeAction()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed);

        business.OpenBusiness(RequiredLevel);

        business.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public void WorldObject_Harvest_UpgradableBusiness_DoesNotCountUpgradeAction()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed);
        business.OpenBusiness(RequiredLevel);
        business.UpdateVisits(10);

        business.Harvest();

        business.UpgradeActionCount.Should().Be(1);
    }

    [Fact]
    public void WorldObject_HarvestFranchise_UpgradableAtRequiredLevel_CountsUpgradeAction()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed);
        business.SetFranchiseLocation(faker.FranchiseLocation(commodityLeft: 10), faker.Random.String2(64));

        business.HarvestFranchise(RequiredLevel);

        business.UpgradeActionCount.Should().Be(1);
    }

    [Fact]
    public void WorldObject_HarvestFranchise_BelowRequiredLevel_DoesNotCountUpgradeAction()
    {
        var faker = new Faker();
        var business = faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed);
        business.SetFranchiseLocation(faker.FranchiseLocation(commodityLeft: 10), faker.Random.String2(64));

        business.HarvestFranchise(RequiredLevel - 1);

        business.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public void WorldObject_Harvest_UpgradableMunicipal_CountsUpgradeAction()
    {
        var faker = new Faker();
        var municipal = faker.WorldObject(itemName: "test_mun_upgradable", className: BuildingClassType.Municipal, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - 3_600_000);

        municipal.Harvest();

        municipal.UpgradeActionCount.Should().Be(1);
    }
}
