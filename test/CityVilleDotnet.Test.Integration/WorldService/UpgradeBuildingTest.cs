using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

// test_bus_upgradable requires level 2 and 5 upgrade actions, then becomes test_bus_upgradable_2 (commodityReq 20) and gives 100 coins
// onUpgrade: resetUpgradeActionCount, visits = maxVisits, closeBusiness + makeHarvestable
[Collection("Database")]
public class UpgradeBuildingTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static UpgradeBuildingRequest CreateUpgradeBuildingRequest(int x, int y)
    {
        return new UpgradeBuildingRequest
        {
            Building = new BuildingUpgradeBuildingRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task UpgradeBuilding_RequirementsMet_UpgradesBusinessReadyToHarvest()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        business.SetUpgradeAction(5);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var updatedBusiness = await Context.Set<WorldObject>().FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);

        updatedBusiness.ItemName.Should().Be("test_bus_upgradable_2");
        updatedBusiness.State.Should().Be(WorldObjectState.ClosedHarvestable);
        updatedBusiness.Visits.Should().Be(20);
        updatedBusiness.UpgradeActionCount.Should().Be(0);
        updatedBusiness.CanHarvest().Should().BeTrue();
        player.Gold.Should().Be(goldBefore + 100);
    }

    [Fact]
    public async Task UpgradeBuilding_NotEnoughUpgradeActions_ReturnsInvalidState()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        business.SetUpgradeAction(4);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.InvalidState);
        business.ItemName.Should().Be("test_bus_upgradable");
        business.UpgradeActionCount.Should().Be(4);
    }

    [Fact]
    public async Task UpgradeBuilding_LevelTooLow_ReturnsInvalidState()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        business.SetUpgradeAction(5);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.InvalidState);
        business.ItemName.Should().Be("test_bus_upgradable");
    }
}
