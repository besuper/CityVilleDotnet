using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.WorldObject;

// test_pier at (10, 10) has berth squares (9, 12) and (9, 9), test_ship at (6, 10) covers x 6..9 and y 10..12
[Collection("Domain")]
public class WorldObjectShipTest
{
    [Fact]
    public void World_CountPierUpgradeActions_OnlyCountsBerthedPiers()
    {
        var faker = new Faker();
        var berthedPier = faker.WorldObject(itemName: "test_pier", className: BuildingClassType.Pier, x: 10, y: 10);
        var farPier = faker.WorldObject(itemName: "test_pier", className: BuildingClassType.Pier, x: 30, y: 30);
        var ship = faker.WorldObject(itemName: "test_ship", className: BuildingClassType.Ship, x: 6, y: 10);
        var world = faker.World(objects: [berthedPier, farPier, ship]);

        world.CountPierUpgradeActions(ship);
        world.CountPierUpgradeActions(ship);

        berthedPier.UpgradeActionCount.Should().Be(2);
        farPier.UpgradeActionCount.Should().BeNull();
        ship.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public void World_CountPierUpgradeActions_DockHouse_CountsNothing()
    {
        var faker = new Faker();
        var dockHouse = faker.WorldObject(itemName: "test_pier", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var ship = faker.WorldObject(itemName: "test_ship", className: BuildingClassType.Ship, x: 6, y: 10);
        var world = faker.World(objects: [dockHouse, ship]);

        world.CountPierUpgradeActions(ship);

        dockHouse.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public void World_CountPierUpgradeActions_UnsupportedShip_CountsNothing()
    {
        var faker = new Faker();
        var pier = faker.WorldObject(itemName: "test_pier", className: BuildingClassType.Pier, x: 10, y: 10);
        var ship = faker.WorldObject(itemName: "test_cargo_ship", className: BuildingClassType.Ship, x: 6, y: 10);
        var world = faker.World(objects: [pier, ship]);

        world.CountPierUpgradeActions(ship);

        pier.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public void WorldObject_BoostPlot_ShipUnderInstantLimit_BecomesGrown()
    {
        var faker = new Faker();
        var ship = faker.WorldObject(itemName: "test_ship", className: BuildingClassType.Ship, contractName: "test_ship_contract", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        ship.BoostPlot();

        ship.State.Should().Be(WorldObjectState.Grown);
        ship.CanHarvest().Should().BeTrue();
    }
}
