using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Factory.Player;

namespace CityVilleDotnet.Test.Domain.Supply;

public class ProcessGoodsTest
{
    private static readonly GameItem DefaultItem = new GameItem
    {
        Name = "test_bus",
        Type = "business",
        Commodity =
        [
            new CommodityItem()
            {
                Name = "goods",
                Default = 1
            },
            new CommodityItem()
            {
                Name = "premium_goods",
                Default = 0
            }
        ],
        Gates = new GatesContainer
        {
            Gates = []
        },
        MasteryItems = [],
        CommodityRequired = 15
    };
    
    private static readonly GameItem ItemNoPremiumGoods = new GameItem
    {
        Name = "test_bus",
        Type = "business",
        Commodity =
        [
            new CommodityItem()
            {
                Name = "goods",
                Default = 1
            }
        ],
        Gates = new GatesContainer
        {
            Gates = []
        },
        MasteryItems = [],
        CommodityRequired = 15
    };

    [Fact]
    public void Player_ProcessGoods_UseGoods()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        player.SetGoods(25);
        player.SetPremiumGoods(500);

        player.ProcessGoods(DefaultItem);

        player.Goods.Should().Be(25 - DefaultItem.CommodityRequired);
    }
    
    [Fact]
    public void Player_ProcessGoods_UseGoodsAndPremiumGoods()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        player.SetGoods(5);
        player.SetPremiumGoods(500);

        player.ProcessGoods(DefaultItem);

        player.Goods.Should().Be(0);
        player.PremiumGoods.Should().Be(490);
    }
    
    [Fact]
    public void Player_ProcessGoods_OnlyUsePremiumGoods()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        player.SetGoods(0);
        player.SetPremiumGoods(500);

        player.ProcessGoods(DefaultItem);

        player.Goods.Should().Be(0);
        player.PremiumGoods.Should().Be(485);
    }
    
    [Fact]
    public void Player_ProcessGoods_NotEnoughGoodsAndPremiumGoods()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Clear();

        player.SetGoods(0);
        player.SetPremiumGoods(0);

        var act = () => player.ProcessGoods(DefaultItem);

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

        player.SetGoods(0);
        player.SetPremiumGoods(500);

        var act = () => player.ProcessGoods(ItemNoPremiumGoods);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        
        player.Goods.Should().Be(0);
        player.PremiumGoods.Should().Be(500);
    }
}