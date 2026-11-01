using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;
using FluorineFx;

namespace CityVilleDotnet.Test.Domain.StockExchange;

// See test_stockexchange and test_contract_stock in TestData/gameSettings.xml
[Collection("Domain")]
public class PlayerStockExchangeTest
{
    private static GameItem Item(string name) => GameSettingsManager.Instance.GetItem(name)!;

    private static GatesItem Gate(string name) => Item("test_stockexchange").GetGates().First(x => x.Name == name);

    private static (CityVilleDotnet.Domain.Entities.Player Player, CityVilleDotnet.Domain.Entities.WorldObject Building) CreatePlayerWithStockExchange(string itemName = "test_stockexchange")
    {
        var faker = new Faker();
        var building = faker.WorldObject(itemName: itemName, className: BuildingClassType.PeepCapacityResource, state: WorldObjectState.Plowed);
        var player = faker.Player(world: faker.World(objects: [building]));
        player.SetGold(0);
        player.SetCash(0);

        return (player, building);
    }

    private static void PlantAndGrow(CityVilleDotnet.Domain.Entities.Player player, CityVilleDotnet.Domain.Entities.WorldObject building)
    {
        player.SetGold(player.Gold + 1500);
        player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_stock"));
        building.SetFullGrown();
    }

    [Fact]
    public void Player_PlantMechanicContract_CoinContract_ChargesCostTimesMultiplierAndPlants()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetGold(2000);

        player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_stock"));

        player.Gold.Should().Be(500);
        building.State.Should().Be(WorldObjectState.Planted);
        building.ContractName.Should().Be("test_contract_stock");
        building.PlantTime.Should().NotBeNull();
    }

    [Fact]
    public void Player_PlantMechanicContract_CashContract_ChargesCash()
    {
        // 5 cash * 1.5 truncated
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetCash(10);

        player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_stock_cash"));

        player.Cash.Should().Be(3);
        building.State.Should().Be(WorldObjectState.Planted);
    }

    [Fact]
    public void Player_PlantMechanicContract_MultiplierBelowOne_ChargesContractCost()
    {
        var (player, building) = CreatePlayerWithStockExchange("test_stockexchange_cheap");
        player.SetGold(2000);

        player.PlantMechanicContract(building, Item("test_stockexchange_cheap"), Item("test_contract_stock"));

        player.Gold.Should().Be(1000);
    }

    [Fact]
    public void Player_PlantMechanicContract_ContractNotPlaceableOnBuilding_Throws()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetGold(2000);

        var act = () => player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_elsewhere"));

        act.Should().Throw<Exception>();
        player.Gold.Should().Be(2000);
        building.State.Should().Be(WorldObjectState.Plowed);
    }

    [Fact]
    public void Player_PlantMechanicContract_AlreadyPlanted_Throws()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetGold(5000);
        player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_stock"));

        var act = () => player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_stock"));

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Player_PlantMechanicContract_NotEnoughCoins_ThrowsNotEnoughMoney()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetGold(1499);

        var act = () => player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_stock"));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        building.State.Should().Be(WorldObjectState.Plowed);
    }

    [Fact]
    public void Player_InstaReadyMechanicContract_JustPlanted_ChargesRemainingHoursTimesMultiplier()
    {
        // growTime 0.5 day = 11.5h, 2 cash per hour => round(~22.99) = 23
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetGold(1500);
        player.SetCash(30);
        player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_stock"));

        player.InstaReadyMechanicContract(building, Item("test_stockexchange"));

        player.Cash.Should().Be(7);
        building.HasGrown().Should().BeTrue();
    }

    [Fact]
    public void Player_InstaReadyMechanicContract_NoContract_Throws()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetCash(30);

        var act = () => player.InstaReadyMechanicContract(building, Item("test_stockexchange"));

        act.Should().Throw<Exception>();
        player.Cash.Should().Be(30);
    }

    [Fact]
    public void Player_HarvestInvestment_FirstHarvest_PaysFirstTimeTableWithHarvestMultiplier()
    {
        // firstTime 300 coins * (1 + 50 / 100)
        var (player, building) = CreatePlayerWithStockExchange();
        PlantAndGrow(player, building);

        var contractName = player.HarvestInvestment(building, Item("test_stockexchange"));

        contractName.Should().Be("test_contract_stock");
        player.Gold.Should().Be(450);
        building.InvestmentReturns.Should().Be(450);
        building.InvestmentCount.Should().Be(1);
        building.State.Should().Be(WorldObjectState.Plowed);
        building.ContractName.Should().BeNull();
    }

    [Fact]
    public void Player_HarvestInvestment_ThreeHarvests_PaysFirstSecondThenDefaultTables()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        var golds = new List<int>();

        for (var i = 0; i < 3; i++)
        {
            PlantAndGrow(player, building);
            var goldBefore = player.Gold;
            player.HarvestInvestment(building, Item("test_stockexchange"));
            golds.Add(player.Gold - goldBefore);
        }

        golds.Should().Equal(450, 300, 150);
        building.InvestmentReturns.Should().Be(900);
        building.InvestmentCount.Should().Be(3);
    }

    [Fact]
    public void Player_HarvestInvestment_WithShares_PaysEachShareWithoutMultiplierAndConsumesThem()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.AddItem("test_contract_stock_share", 3);
        PlantAndGrow(player, building);

        player.HarvestInvestment(building, Item("test_stockexchange"));

        player.Gold.Should().Be(480);
        player.CountInventoryItem("test_contract_stock_share").Should().Be(0);
        building.InvestmentReturns.Should().Be(480);
    }

    [Fact]
    public void Player_HarvestInvestment_NotGrown_Throws()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetGold(1500);
        player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_stock"));

        var act = () => player.HarvestInvestment(building, Item("test_stockexchange"));

        act.Should().Throw<Exception>();
        building.InvestmentCount.Should().Be(0);
    }

    [Fact]
    public void Player_BuyInvestmentGate_NoReturns_ChargesFullCashCostAndCompletesReturns()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetCash(100);

        player.BuyInvestmentGate(building, Gate("upgrade_investment"));

        player.Cash.Should().Be(52);
        building.InvestmentReturns.Should().Be(1000);
    }

    [Fact]
    public void Player_BuyInvestmentGate_PartialReturns_ChargesRemainingRatio()
    {
        // floor(0.75 * 48) = 36
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetCash(100);
        building.CompleteInvestmentReturns(250);

        player.BuyInvestmentGate(building, Gate("upgrade_investment"));

        player.Cash.Should().Be(64);
        building.InvestmentReturns.Should().Be(1000);
    }

    [Fact]
    public void Player_BuyInvestmentGate_AlreadyComplete_ChargesNothingAndKeepsReturns()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetCash(100);
        building.CompleteInvestmentReturns(1500);

        player.BuyInvestmentGate(building, Gate("upgrade_investment"));

        player.Cash.Should().Be(100);
        building.InvestmentReturns.Should().Be(1500);
    }

    [Fact]
    public void Player_BuyInvestmentGate_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetCash(47);

        var act = () => player.BuyInvestmentGate(building, Gate("upgrade_investment"));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        building.InvestmentReturns.Should().Be(0);
    }

    [Fact]
    public void Player_BuyInvestmentGate_StatusGate_Throws()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetCash(100);

        var act = () => player.BuyInvestmentGate(building, Gate("upgrade_status"));

        act.Should().Throw<Exception>();
        player.Cash.Should().Be(100);
    }

    [Fact]
    public void WorldObject_ToDto_PlantedStockExchange_SendsContractInSeconds()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        player.SetGold(1500);
        player.PlantMechanicContract(building, Item("test_stockexchange"), Item("test_contract_stock"));

        var contract = (ASObject)building.ToDto().MechanicData["contract"]!;

        contract["contractName"].Should().Be("test_contract_stock");
        contract["plantTime"].Should().Be(building.PlantTime!.Value / 1000);
    }

    [Fact]
    public void WorldObject_ToDto_HarvestedStockExchange_SendsInvestmentData()
    {
        var (player, building) = CreatePlayerWithStockExchange();
        PlantAndGrow(player, building);
        player.HarvestInvestment(building, Item("test_stockexchange"));

        var dto = building.ToDto();

        dto.MechanicData.ContainsKey("contract").Should().BeFalse();
        var investmentData = (ASObject)dto.MechanicData["investmentData"]!;
        investmentData["coin"].Should().Be(450);
        investmentData["coin_numInvestments"].Should().Be(1);
    }

    [Fact]
    public void WorldObject_ToDto_NeverInvested_SendsNoInvestmentData()
    {
        var (_, building) = CreatePlayerWithStockExchange();

        building.ToDto().MechanicData.ContainsKey("investmentData").Should().BeFalse();
    }
}
