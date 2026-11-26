using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.SpaceShuttle;

// See test_launchpad and test_contract_space in TestData/gameSettings.xml
[Collection("Domain")]
public class PlayerSpaceShuttleTest
{
    private static GameItem Item(string name) => GameSettingsManager.Instance.GetItem(name)!;

    private static (CityVilleDotnet.Domain.Entities.Player Player, CityVilleDotnet.Domain.Entities.WorldObject Building) CreatePlayerWithLaunchPad()
    {
        var faker = new Faker();
        var building = faker.WorldObject(itemName: "test_launchpad", className: BuildingClassType.LaunchPad, state: WorldObjectState.Plowed);
        var player = faker.Player(world: faker.World(objects: [building]));
        player.SetGold(0);
        player.SetCash(0);

        return (player, building);
    }

    [Fact]
    public void Player_PlantMechanicContract_InventoryItemCost_RemovesItemsAndPlants()
    {
        var (player, building) = CreatePlayerWithLaunchPad();
        player.AddItem("test_rocket_fuel", 7);

        player.PlantMechanicContract(building, Item("test_launchpad"), Item("test_contract_space"));

        player.CountInventoryItem("test_rocket_fuel").Should().Be(2);
        building.State.Should().Be(WorldObjectState.Planted);
        building.ContractName.Should().Be("test_contract_space");
    }

    [Fact]
    public void Player_PlantMechanicContract_NotEnoughInventoryItems_ThrowsAndKeepsPlowed()
    {
        var (player, building) = CreatePlayerWithLaunchPad();
        player.AddItem("test_rocket_fuel", 4);

        var act = () => player.PlantMechanicContract(building, Item("test_launchpad"), Item("test_contract_space"));

        act.Should().Throw<Exception>();
        player.CountInventoryItem("test_rocket_fuel").Should().Be(4);
        building.State.Should().Be(WorldObjectState.Plowed);
    }

    [Fact]
    public void Player_HarvestMechanicContract_FriendContract_PaysContractTableAndPlows()
    {
        var (player, building) = CreatePlayerWithLaunchPad();
        player.AddItem("test_rocket_fuel", 5);
        player.PlantMechanicContract(building, Item("test_launchpad"), Item("test_contract_space"));
        building.SetFullGrown();
        var mechanic = Item("test_launchpad").Mechanics!.GetMechanicByGameMode("GMPlay")!.GetMechanicItemByType("contractHarvest")!;

        var contractName = player.HarvestMechanicContract(building, Item("test_launchpad"), mechanic);

        contractName.Should().Be("test_contract_space");
        player.Gold.Should().Be(70);
        building.State.Should().Be(WorldObjectState.Plowed);
        building.ContractName.Should().BeNull();
    }
}
