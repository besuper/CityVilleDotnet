using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.LotOrder;

[Collection("Domain")]
public class LotOrderPaymentTest
{
    [Fact]
    public void Player_PayLotOrder_CoinCost_RemovesCoinsOnly()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetGold(1000);
        player.SetCash(10);

        player.PayLotOrder(GameSettingsManager.Instance.GetItem("test_lot_coins")!);

        player.Gold.Should().Be(800);
        player.Cash.Should().Be(10);
    }

    [Fact]
    public void Player_PayLotOrder_CashOnly_RemovesCash()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetGold(1000);
        player.SetCash(10);

        player.PayLotOrder(GameSettingsManager.Instance.GetItem("test_lot_cash")!);

        player.Gold.Should().Be(1000);
        player.Cash.Should().Be(6);
    }

    [Fact]
    public void Player_PayLotOrder_NotEnoughCoins_ThrowsNotEnoughMoney()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetGold(100);

        var act = () => player.PayLotOrder(GameSettingsManager.Instance.GetItem("test_lot_coins")!);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
    }

    [Fact]
    public void Player_RefundLotOrder_CashOnly_RefundsNothing()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetGold(1000);
        player.SetCash(10);

        player.RefundLotOrder(GameSettingsManager.Instance.GetItem("test_lot_cash")!);

        player.Gold.Should().Be(1000);
        player.Cash.Should().Be(10);
    }

    [Fact]
    public void Player_RefundLotOrder_CoinCost_RefundsCoins()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetGold(1000);

        player.RefundLotOrder(GameSettingsManager.Instance.GetItem("test_lot_coins")!);

        player.Gold.Should().Be(1200);
    }
}
