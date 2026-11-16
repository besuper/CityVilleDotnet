using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.WorldObject;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.Gas;

// See test_auto_res, test_gas_* in TestData/gameSettings.xml and qm_test_gas_reward, qm_test_auto_harvest in TestData/questSettings.xml
[Collection("Domain")]
public class PlayerGasTest
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);
    private static readonly TimeSpan RegenInterval = TimeSpan.FromDays(1);

    private static FakeTimeProvider UseFakeTime()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        return time;
    }

    private static List<global::CityVilleDotnet.Domain.Entities.WorldObject> AutoResidences(Faker faker, int count, string itemName = "test_auto_res")
    {
        return Enumerable.Range(0, count).Select(_ => faker.WorldObject(itemName: itemName, className: BuildingClassType.Residence)).ToList();
    }

    [Fact]
    public void Player_GetCurrentGas_NewPlayer_ReturnsRegenCap()
    {
        var faker = new Faker();
        var player = faker.Player();

        var (gas, _) = player.GetCurrentGas(StartTime.ToUnixTimeSeconds());

        gas.Should().Be(12);
    }

    [Fact]
    public void Player_PayAutomation_TwoObjects_RemovesGasAndEnergy()
    {
        UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(2);

        var energyBefore = player.Energy;

        player.PayAutomation(AutoResidences(faker, 2), StartTime.ToUnixTimeSeconds());

        player.Gas.Should().Be(10);
        player.Energy.Should().Be(energyBefore - 4);
    }

    [Fact]
    public void Player_PayAutomation_FiveObjects_AppliesEnergySavings()
    {
        UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(2);

        var energyBefore = player.Energy;

        player.PayAutomation(AutoResidences(faker, 5), StartTime.ToUnixTimeSeconds());

        player.Gas.Should().Be(7);
        player.Energy.Should().Be(energyBefore - 8);
    }

    [Fact]
    public void Player_PayAutomation_BelowRequiredLevel_ThrowsInvalidState()
    {
        var faker = new Faker();
        var player = faker.Player();

        var act = () => player.PayAutomation(AutoResidences(faker, 1), StartTime.ToUnixTimeSeconds());

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void Player_PayAutomation_NotEnoughGas_ThrowsForceReload()
    {
        UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(2);

        var act = () => player.PayAutomation(AutoResidences(faker, 13, "res_cottage3"), StartTime.ToUnixTimeSeconds());

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.ForceReload);
    }

    [Fact]
    public void Player_PayAutomation_FromRegenCap_RestartsRegenTimer()
    {
        var time = UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();
        player.SetLevel(2);

        player.PayAutomation(AutoResidences(faker, 1, "res_cottage3"), time.GetUtcNow().ToUnixTimeSeconds());

        time.Advance(RegenInterval - TimeSpan.FromSeconds(1));
        player.GetCurrentGas(time.GetUtcNow().ToUnixTimeSeconds()).Gas.Should().Be(11);

        time.Advance(TimeSpan.FromSeconds(1));
        player.GetCurrentGas(time.GetUtcNow().ToUnixTimeSeconds()).Gas.Should().Be(15);
    }

    [Fact]
    public void Player_AddGas_AddsOnTopOfRegeneratedGas()
    {
        UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();

        player.AddGas(5);

        player.Gas.Should().Be(17);
    }

    [Fact]
    public void Player_BuyGas_CashItem_RemovesCashAndAddsGas()
    {
        UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();
        var cashBefore = player.Cash;

        player.BuyGas("test_gas_cash");

        player.Cash.Should().Be(cashBefore - 5);
        player.Gas.Should().Be(17);
    }

    [Fact]
    public void Player_BuyGas_CoinItem_RemovesCoinsAndAddsGas()
    {
        UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();
        var goldBefore = player.Gold;

        player.BuyGas("test_gas_coins");

        player.Gold.Should().Be(goldBefore - 100);
        player.Gas.Should().Be(15);
    }

    [Fact]
    public void Player_BuyGas_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();
        player.RemoveCash(player.Cash);

        var act = () => player.BuyGas("test_gas_cash");

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
    }

    [Fact]
    public void Player_BuyGas_ItemWithoutGasReward_ThrowsInvalidData()
    {
        var faker = new Faker();
        var player = faker.Player();

        var act = () => player.BuyGas("deco_tree");

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidData);
    }

    [Fact]
    public void Player_AddItem_GasConsumeItem_AddsGasInsteadOfInventory()
    {
        UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();

        player.AddItem("test_gas_consume");

        player.Gas.Should().Be(13);
        player.CountInventoryItem("test_gas_consume").Should().Be(0);
    }

    [Fact]
    public void Quest_ClaimRewards_GasReward_AddsGas()
    {
        UseFakeTime();
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_gas_reward", length: 1);

        quest.ClaimRewards(player);

        player.Gas.Should().Be(22);
    }

    [Fact]
    public void Player_HandleQuestsProgress_AutoHarvest_AddsAmount()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_auto_harvest", length: 2);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("autoHarvest", amount: 3);

        quest.Progress[0].Should().Be(3);
    }
}
