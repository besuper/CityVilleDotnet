using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.WorldObject;

// test_dock_house at (10, 10) has berth squares (-1, 2) and (-1, -1) => absolute (9, 12) and (9, 9)
// test_cruise_ship is 3x2, swapped to 2x3 when its direction is odd
[Collection("Domain")]
public class WorldObjectCruiseUpgradeActionTest
{
    [Fact]
    public void GameItem_GetBerthSquares_ParsesPointVector()
    {
        var dockHouse = GameSettingsManager.Instance.GetItem("test_dock_house")!;

        dockHouse.GetBerthSquares().Should().Equal((-1, 2), (-1, -1));
    }

    [Fact]
    public void GameItem_GetBerthSquares_NoBerthSquares_ReturnsEmpty()
    {
        GameSettingsManager.Instance.GetItem("test_cruise_ship")!.GetBerthSquares().Should().BeEmpty();
    }

    [Fact]
    public void GameItem_SupportsShip_MatchesSupportedShipKeywords()
    {
        var dockHouse = GameSettingsManager.Instance.GetItem("test_dock_house")!;

        dockHouse.SupportsShip(GameSettingsManager.Instance.GetItem("test_cruise_ship")!).Should().BeTrue();
        dockHouse.SupportsShip(GameSettingsManager.Instance.GetItem("test_cargo_ship")!).Should().BeFalse();
    }

    [Fact]
    public void WorldObject_IsBerthedAt_BerthSquareOnShipEdge_ReturnsTrue()
    {
        // Ship at (6, 10) covers x 6..9 and y 10..12 because BasePier::isValidShipBerth is inclusive
        var faker = new Faker();
        var dockHouse = faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var ship = faker.WorldObject(itemName: "test_cruise_ship", className: BuildingClassType.HarvestableShip, x: 6, y: 10);

        ship.IsBerthedAt(dockHouse).Should().BeTrue();
    }

    [Fact]
    public void WorldObject_IsBerthedAt_FarFromBerthSquares_ReturnsFalse()
    {
        var faker = new Faker();
        var dockHouse = faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var ship = faker.WorldObject(itemName: "test_cruise_ship", className: BuildingClassType.HarvestableShip, x: 20, y: 20);

        ship.IsBerthedAt(dockHouse).Should().BeFalse();
    }

    [Fact]
    public void WorldObject_IsBerthedAt_RotatedShip_UsesSwappedSize()
    {
        // At (6, 7) the ship reaches x 9 when it is 3 wide but only x 8 when rotated to 2 wide
        var faker = new Faker();
        var dockHouse = faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var ship = faker.WorldObject(itemName: "test_cruise_ship", className: BuildingClassType.HarvestableShip, x: 6, y: 7, direction: 0);
        var rotatedShip = faker.WorldObject(itemName: "test_cruise_ship", className: BuildingClassType.HarvestableShip, x: 6, y: 7, direction: 1);

        ship.IsBerthedAt(dockHouse).Should().BeTrue();
        rotatedShip.IsBerthedAt(dockHouse).Should().BeFalse();
    }

    [Fact]
    public void WorldObject_IsBerthedAt_UnsupportedShip_ReturnsFalse()
    {
        var faker = new Faker();
        var dockHouse = faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var ship = faker.WorldObject(itemName: "test_cargo_ship", className: BuildingClassType.HarvestableShip, x: 6, y: 10);

        ship.IsBerthedAt(dockHouse).Should().BeFalse();
    }

    [Fact]
    public void WorldObject_CountCruiseUpgradeAction_AlaskanCruise_CountsUpgradeAction()
    {
        var faker = new Faker();
        var dockHouse = faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse);

        dockHouse.CountCruiseUpgradeAction("cruise_contract_alaskan");
        dockHouse.CountCruiseUpgradeAction("cruise_contract_scandinavian");

        dockHouse.UpgradeActionCount.Should().Be(2);
        dockHouse.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public void WorldObject_CountCruiseUpgradeAction_CaribbeanCruise_CountsUpgradeAction2()
    {
        var faker = new Faker();
        var dockHouse = faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse);

        dockHouse.CountCruiseUpgradeAction("cruise_contract_caribbean");
        dockHouse.CountCruiseUpgradeAction("cruise_contract_mediterranean");

        dockHouse.UpgradeActionCount.Should().BeNull();
        dockHouse.UpgradeActionCount2.Should().Be(2);
    }

    [Fact]
    public void WorldObject_CountCruiseUpgradeAction_OtherCruise_CountsNothing()
    {
        var faker = new Faker();
        var dockHouse = faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse);

        dockHouse.CountCruiseUpgradeAction("cruise_contract_aroundTheWorld");

        dockHouse.UpgradeActionCount.Should().BeNull();
        dockHouse.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public void World_CountCruiseUpgradeActions_OnlyCountsBerthedDockHouses()
    {
        var faker = new Faker();
        var berthedDockHouse = faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 10, y: 10);
        var farDockHouse = faker.WorldObject(itemName: "test_dock_house", className: BuildingClassType.DockHouse, x: 30, y: 30);
        var ship = faker.WorldObject(itemName: "test_cruise_ship", className: BuildingClassType.HarvestableShip, x: 6, y: 10);
        var world = faker.World(objects: [berthedDockHouse, farDockHouse, ship]);

        world.CountCruiseUpgradeActions(ship, "cruise_contract_alaskan");

        berthedDockHouse.UpgradeActionCount.Should().Be(1);
        farDockHouse.UpgradeActionCount.Should().BeNull();
        ship.UpgradeActionCount.Should().BeNull();
    }
}
