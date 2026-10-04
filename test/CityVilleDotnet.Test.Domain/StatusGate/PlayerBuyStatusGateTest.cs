using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.StatusGate;

// WorldObjectStatusGate::computeTotalCost: max(floor((1 - progress) * cashCost), 1), 0 when complete or without cashCost
[Collection("Domain")]
public class PlayerBuyStatusGateTest
{
    private static (CityVilleDotnet.Domain.Entities.Player Player, CityVilleDotnet.Domain.Entities.WorldObject Building) CreatePlayerWithBuilding(int? upgradeActionCount, int cash)
    {
        var faker = new Faker();
        var building = faker.WorldObject(itemName: "test_status_gate", className: BuildingClassType.Business);

        if (upgradeActionCount is not null)
            building.SetUpgradeAction(upgradeActionCount.Value);

        var player = faker.Player(world: faker.World(objects: [building]));
        player.SetCash(cash);

        return (player, building);
    }

    [Fact]
    public void Player_BuyStatusGate_NoProgress_ChargesFullCostAndCompletesKey()
    {
        var (player, building) = CreatePlayerWithBuilding(null, 100);

        player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("upgrade_status")!);

        player.Cash.Should().Be(50);
        building.UpgradeActionCount.Should().Be(10);
    }

    [Fact]
    public void Player_BuyStatusGate_PartialProgress_ChargesRemainingProgressRatio()
    {
        var (player, building) = CreatePlayerWithBuilding(3, 100);

        player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("upgrade_status")!);

        player.Cash.Should().Be(65);
        building.UpgradeActionCount.Should().Be(10);
    }

    [Fact]
    public void Player_BuyStatusGate_FloatingPointProgress_MatchesClientRounding()
    {
        // floor((1 - (1 - 1 / 10)) * 50) = floor(4.99...) = 4
        var (player, building) = CreatePlayerWithBuilding(9, 100);

        player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("upgrade_status")!);

        player.Cash.Should().Be(96);
        building.UpgradeActionCount.Should().Be(10);
    }

    [Fact]
    public void Player_BuyStatusGate_FractionalCost_FloorsCost()
    {
        // harvest_status: 4 harvests for 25 cash, 1 done => floor(0.75 * 25) = 18
        var (player, building) = CreatePlayerWithBuilding(1, 100);

        player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("harvest_status")!);

        player.Cash.Should().Be(82);
        building.UpgradeActionCount.Should().Be(4);
    }

    [Fact]
    public void Player_BuyStatusGate_AlreadyComplete_ChargesNothingAndKeepsCount()
    {
        var (player, building) = CreatePlayerWithBuilding(12, 100);

        player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("upgrade_status")!);

        player.Cash.Should().Be(100);
        building.UpgradeActionCount.Should().Be(12);
    }

    [Fact]
    public void Player_BuyStatusGate_WithoutCashCost_ChargesNothing()
    {
        var (player, building) = CreatePlayerWithBuilding(null, 100);

        player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("free_status")!);

        player.Cash.Should().Be(100);
        building.UpgradeActionCount.Should().Be(3);
    }

    [Fact]
    public void Player_BuyStatusGate_UpgradeActionCount2_CompletesOnlyUpgradeActionCount2()
    {
        var (player, building) = CreatePlayerWithBuilding(5, 100);

        player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("upgrade_status2")!);

        player.Cash.Should().Be(50);
        building.UpgradeActionCount2.Should().Be(10);
        building.UpgradeActionCount.Should().Be(5);
    }

    [Fact]
    public void Player_BuyStatusGate_UpgradeActionCount_DoesNotCompleteUpgradeActionCount2()
    {
        var (player, building) = CreatePlayerWithBuilding(null, 100);

        player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("upgrade_status")!);

        building.UpgradeActionCount.Should().Be(10);
        building.UpgradeActionCount2.Should().BeNull();
    }

    [Fact]
    public void Player_BuyStatusGate_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var (player, building) = CreatePlayerWithBuilding(null, 10);

        var act = () => player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("upgrade_status")!);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        player.Cash.Should().Be(10);
        building.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public void Player_BuyStatusGate_UnsupportedKey_ThrowsException()
    {
        var (player, building) = CreatePlayerWithBuilding(null, 100);

        var act = () => player.BuyStatusGate(building, GameSettingsManager.Instance.GetItem("test_status_gate")!.GetStatusGate("corp_status")!);

        act.Should().Throw<Exception>().WithMessage("*corpBusinessCount*");
        player.Cash.Should().Be(100);
    }

    [Fact]
    public void GameItem_GetStatusGate_NonStatusGate_ReturnsNull()
    {
        var gameItem = GameSettingsManager.Instance.GetItem("test_status_gate")!;

        gameItem.GetStatusGate("inventory_gate").Should().BeNull();
        gameItem.GetStatusGate("unknown_gate").Should().BeNull();
        gameItem.GetStatusGate("upgrade_status")!.CashCost.Should().Be(50);
    }
}
