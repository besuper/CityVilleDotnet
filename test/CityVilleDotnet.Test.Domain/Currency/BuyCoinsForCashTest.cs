using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Currency;

[Collection("Domain")]
public class BuyCoinsForCashTest
{
    private static int CoinsForCash => GameSettingsManager.Instance.GetSettings().CoinsForCash;

    [Fact]
    public void Player_BuyCoinsForCash_EnoughCash_ConvertsCashToCoins()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetGold(500);
        player.SetCash(10);

        player.BuyCoinsForCash(3);

        player.Cash.Should().Be(7);
        player.Gold.Should().Be(500 + 3 * CoinsForCash);
    }

    [Fact]
    public void Player_BuyCoinsForCash_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetGold(500);
        player.SetCash(2);

        var act = () => player.BuyCoinsForCash(3);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        player.Cash.Should().Be(2);
        player.Gold.Should().Be(500);
    }
}
