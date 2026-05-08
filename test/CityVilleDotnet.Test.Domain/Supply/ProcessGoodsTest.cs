using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Supply;

[Collection("Domain")]
public class ProcessGoodsTest(DomainFixture fixture)
{
    [Fact]
    public void Player_ProcessGoods_UseGoods()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        var defaultItem = GameSettingsManager.Instance.GetItem("test_bus");

        defaultItem.Should().NotBeNull();

        player.SetGoods(25);
        player.SetPremiumGoods(500);

        player.ProcessGoods(defaultItem);

        player.Goods.Should().Be(25 - defaultItem.CommodityRequired);
    }

    [Fact]
    public void Player_ProcessGoods_UseGoodsAndPremiumGoods()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        var defaultItem = GameSettingsManager.Instance.GetItem("test_bus");

        defaultItem.Should().NotBeNull();

        player.SetGoods(5);
        player.SetPremiumGoods(500);

        player.ProcessGoods(defaultItem);

        player.Goods.Should().Be(0);
        player.PremiumGoods.Should().Be(490);
    }

    [Fact]
    public void Player_ProcessGoods_OnlyUsePremiumGoods()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        var defaultItem = GameSettingsManager.Instance.GetItem("test_bus");

        defaultItem.Should().NotBeNull();

        player.SetGoods(0);
        player.SetPremiumGoods(500);

        player.ProcessGoods(defaultItem);

        player.Goods.Should().Be(0);
        player.PremiumGoods.Should().Be(485);
    }

    [Fact]
    public void Player_ProcessGoods_NotEnoughGoodsAndPremiumGoods()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        var defaultItem = GameSettingsManager.Instance.GetItem("test_bus");

        defaultItem.Should().NotBeNull();

        player.SetGoods(0);
        player.SetPremiumGoods(0);

        var act = () => player.ProcessGoods(defaultItem);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);

        player.Goods.Should().Be(0);
        player.PremiumGoods.Should().Be(0);
    }

    [Fact]
    public void Player_ProcessGoods_NotEnoughGoodsNoPremiumGoods()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        var defaultItem = GameSettingsManager.Instance.GetItem("test_bus_goods");

        defaultItem.Should().NotBeNull();

        player.SetGoods(0);
        player.SetPremiumGoods(500);

        var act = () => player.ProcessGoods(defaultItem);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);

        player.Goods.Should().Be(0);
        player.PremiumGoods.Should().Be(500);
    }

    [Fact]
    public void Player_ProcessGoods_OnlyUsePremiumGoods_PremiumItem()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        var defaultItem = GameSettingsManager.Instance.GetItem("test_bus_premium_goods");

        defaultItem.Should().NotBeNull();

        player.SetGoods(0);
        player.SetPremiumGoods(500);

        player.ProcessGoods(defaultItem);

        player.Goods.Should().Be(0);
        player.PremiumGoods.Should().Be(485);
    }

    [Fact]
    public void Player_ProcessGoods_OnlyUsePremiumGoods_NotEnoughPremiumGoods_PremiumItem()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        var defaultItem = GameSettingsManager.Instance.GetItem("test_bus_premium_goods");

        defaultItem.Should().NotBeNull();

        player.SetGoods(0);
        player.SetPremiumGoods(5);

        var act = () => player.ProcessGoods(defaultItem);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);

        player.Goods.Should().Be(0);
        player.PremiumGoods.Should().Be(5);
    }

    [Fact]
    public void Player_ProcessGoods_UseGoods_UpgradedBusiness()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        var defaultItem = GameSettingsManager.Instance.GetItem("test_bus_2");

        defaultItem.Should().NotBeNull();

        player.SetGoods(150);
        player.SetPremiumGoods(500);

        player.ProcessGoods(defaultItem);

        player.Goods.Should().Be(100);
        player.PremiumGoods.Should().Be(500);
    }
}