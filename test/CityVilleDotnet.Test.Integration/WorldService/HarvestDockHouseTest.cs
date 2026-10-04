using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class HarvestDockHouseTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static HarvestRequest CreateHarvestRequest(int x, int y)
    {
        return new HarvestRequest
        {
            Building = new BuildingHarvestRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task Harvest_AlaskanCruiseBerthedAtDockHouse_PersistsUpgradeActionCount()
    {
        var dockHouse = Faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var ship = Faker.WorldObject(itemName: "test_cruise_ship", className: BuildingClassType.HarvestableShip, contractName: "cruise_contract_alaskan", state: WorldObjectState.Grown, x: 6, y: 10);
        var player = Faker.Player(world: Faker.World(objects: [dockHouse, ship]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(CreateHarvestRequest(6, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedDockHouse = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == dockHouse.Id, TestContext.Current.CancellationToken);
        storedDockHouse.UpgradeActionCount.Should().Be(1);
        storedDockHouse.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public async Task Harvest_CaribbeanCruiseBerthedAtDockHouse_PersistsUpgradeActionCount2()
    {
        var dockHouse = Faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var ship = Faker.WorldObject(itemName: "test_cruise_ship", className: BuildingClassType.HarvestableShip, contractName: "cruise_contract_caribbean", state: WorldObjectState.Grown, x: 6, y: 10);
        var player = Faker.Player(world: Faker.World(objects: [dockHouse, ship]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(CreateHarvestRequest(6, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedDockHouse = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == dockHouse.Id, TestContext.Current.CancellationToken);
        storedDockHouse.UpgradeActionCount.Should().BeNull();
        storedDockHouse.UpgradeActionCount2.Should().Be(1);
    }

    [Fact]
    public async Task Harvest_CruiseShipNotBerthed_DoesNotCountDockHouse()
    {
        var dockHouse = Faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var ship = Faker.WorldObject(itemName: "test_cruise_ship", className: BuildingClassType.HarvestableShip, contractName: "cruise_contract_alaskan", state: WorldObjectState.Grown, x: 20, y: 20);
        var player = Faker.Player(world: Faker.World(objects: [dockHouse, ship]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(CreateHarvestRequest(20, 20), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedDockHouse = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == dockHouse.Id, TestContext.Current.CancellationToken);
        storedDockHouse.UpgradeActionCount.Should().BeNull();
        storedDockHouse.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public async Task Harvest_OtherCruiseBerthedAtDockHouse_DoesNotCountDockHouse()
    {
        var dockHouse = Faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var ship = Faker.WorldObject(itemName: "test_cruise_ship", className: BuildingClassType.HarvestableShip, contractName: "cruise_contract_aroundTheWorld", state: WorldObjectState.Grown, x: 6, y: 10);
        var player = Faker.Player(world: Faker.World(objects: [dockHouse, ship]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(CreateHarvestRequest(6, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedDockHouse = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == dockHouse.Id, TestContext.Current.CancellationToken);
        storedDockHouse.UpgradeActionCount.Should().BeNull();
        storedDockHouse.UpgradeActionCount2.Should().BeNull();
    }
}
