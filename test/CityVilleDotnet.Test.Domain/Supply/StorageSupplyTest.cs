using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using FluorineFx;
using WorldEntity = CityVilleDotnet.Domain.Entities.World;
using WorldObjectEntity = CityVilleDotnet.Domain.Entities.WorldObject;

namespace CityVilleDotnet.Test.Domain.Supply;

// See test_supply_mall* and test_mall_shop_* in TestData/gameSettings.xml
// test_mall_shop_a: 10 goods, 1 harvest energy - test_mall_shop_b: 20 premium goods, 2 harvest energy - both pay 5 coins per customer
[Collection("Domain")]
public class StorageSupplyTest
{
    private const int MallId = 1;

    private static GameItem Item(string name)
    {
        return GameSettingsManager.Instance.GetItem(name)!;
    }

    private static (WorldEntity World, WorldObjectEntity Mall, List<WorldObjectEntity> Shops) Setup(Faker faker, string mallName, params string[] shopNames)
    {
        var mall = faker.WorldObject(itemName: mallName, className: BuildingClassType.Mall, worldFlatId: MallId);
        var shops = shopNames.Select((name, i) => faker.WorldObject(itemName: name, className: BuildingClassType.Business, worldFlatId: 10 + i)).ToList();

        for (var i = 0; i < shops.Count; i++)
        {
            shops[i].StoreIn(MallId, i);
        }

        return (faker.World(objects: [mall, .. shops]), mall, shops);
    }

    private static (WorldEntity World, WorldObjectEntity Mall) SetupSupplied(Faker faker, string mallName, params string[] shopNames)
    {
        var (world, mall, _) = Setup(faker, mallName, shopNames);
        mall.OpenStorageSupply(world.GetStoredObjects(mall));

        return (world, mall);
    }

    [Fact]
    public void WorldObject_GetStorageSupplyCommodities_SumsRequirementsByCommodity()
    {
        var faker = new Faker();
        var (_, _, shops) = Setup(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b", "test_mall_shop_a");

        var commodities = WorldObjectEntity.GetStorageSupplyCommodities(shops);

        commodities.Should().BeEquivalentTo([("goods", 20), ("premium_goods", 20)]);
    }

    [Fact]
    public void Player_SupplyStorage_PaysEachCommodityOfStoredShops()
    {
        var faker = new Faker();
        var (world, mall, _) = Setup(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b");
        var player = faker.Player(world: world);
        player.SetGoods(100);
        player.SetPremiumGoods(100);

        player.SupplyStorage(world.GetStoredObjects(mall));

        player.Goods.Should().Be(90);
        player.PremiumGoods.Should().Be(80);
    }

    [Fact]
    public void Player_SupplyStorage_NotEnoughPremiumGoods_ThrowsWithoutCompletingWithOtherGoods()
    {
        var faker = new Faker();
        var (world, mall, _) = Setup(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b");
        var player = faker.Player(world: world);
        player.SetGoods(1000);
        player.SetPremiumGoods(19);

        var act = () => player.SupplyStorage(world.GetStoredObjects(mall));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        player.Goods.Should().Be(1000);
        player.PremiumGoods.Should().Be(19);
    }

    [Fact]
    public void Player_SupplyStorage_NoStoredShop_ThrowsInvalidState()
    {
        var faker = new Faker();
        var (world, mall, _) = Setup(faker, "test_supply_mall");
        var player = faker.Player(world: world);

        var act = () => player.SupplyStorage(world.GetStoredObjects(mall));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void Player_SupplyStorage_CountsUpgradeActionsOnStoredShops()
    {
        var faker = new Faker();
        var (world, mall, shops) = Setup(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b");
        var player = faker.Player(world: world);
        player.SetLevel(15);
        player.SetGoods(100);
        player.SetPremiumGoods(100);

        player.SupplyStorage(world.GetStoredObjects(mall));

        shops[0].UpgradeActionCount.Should().Be(1);
        shops[1].UpgradeActionCount.Should().BeNull();
        mall.UpgradeActionCount.Should().BeNull();
    }

    [Fact]
    public void WorldObject_OpenStorageSupply_RecordsShopsInSlotOrder()
    {
        var faker = new Faker();

        var (_, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_b", "test_mall_shop_a");

        mall.IsStorageSupplied().Should().BeTrue();
        mall.GetSupplyResourceNames().Should().Equal("test_mall_shop_b", "test_mall_shop_a");
        mall.GetSupplyCustomersReq().Should().Be(30);
        mall.State.Should().Be(WorldObjectState.Open);
        mall.Visits.Should().Be(0);
    }

    [Fact]
    public void WorldObject_OpenStorageSupply_AlreadySupplied_ThrowsException()
    {
        var faker = new Faker();
        var (world, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a");

        var act = () => mall.OpenStorageSupply(world.GetStoredObjects(mall));

        act.Should().Throw<Exception>();
        mall.GetSupplyResourceNames().Should().Equal("test_mall_shop_a");
    }

    [Fact]
    public void WorldObject_OpenStorageSupply_OpenMallWithoutResources_Supplies()
    {
        var faker = new Faker();
        var mall = faker.WorldObject(itemName: "test_supply_mall", className: BuildingClassType.Mall, state: WorldObjectState.Open, worldFlatId: MallId);
        var shop = faker.WorldObject(itemName: "test_mall_shop_a", className: BuildingClassType.Business, worldFlatId: 10);
        shop.StoreIn(MallId, 0);
        var world = faker.World(objects: [mall, shop]);

        mall.OpenStorageSupply(world.GetStoredObjects(mall));

        mall.GetSupplyResourceNames().Should().Equal("test_mall_shop_a");
    }

    [Fact]
    public void World_PlaceFromStorage_SuppliedMall_RemovesLastOccurrenceOfTheShop()
    {
        var faker = new Faker();
        var (world, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b", "test_mall_shop_a");

        world.PlaceFromStorage(mall, 0, 20, 20, 0);

        mall.GetSupplyResourceNames().Should().Equal("test_mall_shop_a", "test_mall_shop_b");
        mall.GetSupplyCustomersReq().Should().Be(30);
        mall.State.Should().Be(WorldObjectState.Open);
    }

    [Fact]
    public void World_PlaceFromStorage_ShopStoredAfterSupplyStillThere_KeepsResources()
    {
        var faker = new Faker();
        var (world, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a");
        var newShop = faker.WorldObject(itemName: "test_mall_shop_a", className: BuildingClassType.Business, worldFlatId: 50);
        world.AddBuilding(newShop);
        world.StoreObject(mall, Item("test_supply_mall"), newShop);

        world.PlaceFromStorage(mall, 0, 20, 20, 0);

        mall.GetSupplyResourceNames().Should().Equal("test_mall_shop_a");
        mall.State.Should().Be(WorldObjectState.Open);
    }

    [Fact]
    public void World_PlaceFromStorage_LastSuppliedShop_ClosesSupply()
    {
        var faker = new Faker();
        var (world, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a");
        mall.UpdateVisits(3);

        world.PlaceFromStorage(mall, 0, 20, 20, 0);

        mall.IsStorageSupplied().Should().BeFalse();
        mall.State.Should().Be(WorldObjectState.Static);
        mall.Visits.Should().Be(0);
    }

    [Fact]
    public void World_PlaceFromStorage_VisitsReachNewCustomersReq_BecomesHarvestable()
    {
        var faker = new Faker();
        var (world, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b");
        mall.UpdateVisits(15);

        world.PlaceFromStorage(mall, 1, 20, 20, 0);

        mall.GetSupplyCustomersReq().Should().Be(10);
        mall.State.Should().Be(WorldObjectState.ClosedHarvestable);
    }

    [Fact]
    public void World_PlaceFromStorage_NotSuppliedMall_KeepsState()
    {
        var faker = new Faker();
        var (world, mall, _) = Setup(faker, "test_supply_mall", "test_mall_shop_a");

        world.PlaceFromStorage(mall, 0, 20, 20, 0);

        mall.IsStorageSupplied().Should().BeFalse();
        mall.State.Should().Be(WorldObjectState.Static);
    }

    [Fact]
    public void WorldObject_UpdateVisits_SuppliedMall_HarvestableAtCustomersReq()
    {
        var faker = new Faker();
        var (_, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b");

        mall.UpdateVisits(29).Should().BeTrue();
        mall.State.Should().Be(WorldObjectState.Open);

        mall.UpdateVisits(1).Should().BeTrue();
        mall.State.Should().Be(WorldObjectState.ClosedHarvestable);
    }

    [Fact]
    public void WorldObject_UpdateVisits_OpenMallWithoutResources_Refused()
    {
        var faker = new Faker();
        var mall = faker.WorldObject(itemName: "test_supply_mall", className: BuildingClassType.Mall, state: WorldObjectState.Open);

        mall.UpdateVisits(5).Should().BeFalse();
        mall.State.Should().Be(WorldObjectState.Open);
    }

    [Fact]
    public void WorldObject_GetStorageSupplyHarvestEnergyCost_SumsMallAndSuppliedShops()
    {
        var faker = new Faker();
        var (_, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b");

        mall.GetStorageSupplyHarvestEnergyCost(Item("test_supply_mall")).Should().Be(4);
    }

    [Fact]
    public void WorldObject_GetStorageSupplyHarvestEnergyCost_DiscountGreaterThanCost_CostsOne()
    {
        var faker = new Faker();
        var (_, mall) = SetupSupplied(faker, "test_supply_mall_premium", "test_mall_shop_a", "test_mall_shop_b");

        mall.GetStorageSupplyHarvestEnergyCost(Item("test_supply_mall_premium")).Should().Be(1);
    }

    [Fact]
    public void WorldObject_GetStorageSupplyHarvestEnergyCost_ZeroDiscount_KeepsFullCost()
    {
        var faker = new Faker();
        var (_, mall) = SetupSupplied(faker, "test_supply_mall_premium", "test_mall_shop_a");

        mall.GetStorageSupplyHarvestEnergyCost(Item("test_supply_mall_premium")).Should().Be(2);
    }

    [Fact]
    public void WorldObject_Harvest_SuppliedMall_ResetsSupply()
    {
        var faker = new Faker();
        var (_, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a");
        mall.UpdateVisits(10);

        mall.Harvest();

        mall.IsStorageSupplied().Should().BeFalse();
        mall.State.Should().Be(WorldObjectState.Static);
        mall.Visits.Should().Be(0);
    }

    [Fact]
    public void Player_CollectStorageSupplyRewards_PaysShopCoinsTimesCommodityReq()
    {
        var faker = new Faker();
        var (world, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b");
        var player = faker.Player(world: world);
        var goldBefore = player.Gold;

        player.CollectStorageSupplyRewards(Item("test_supply_mall"), mall.GetSupplyResourceNames());

        player.Gold.Should().Be(goldBefore + 5 * 10 + 5 * 20);
    }

    [Fact]
    public void Player_CollectStorageSupplyRewards_PremiumMall_AppliesCommodityBonus()
    {
        var faker = new Faker();
        var (world, mall) = SetupSupplied(faker, "test_supply_mall_premium", "test_mall_shop_a", "test_mall_shop_b");
        var player = faker.Player(world: world);
        var goldBefore = player.Gold;

        player.CollectStorageSupplyRewards(Item("test_supply_mall_premium"), mall.GetSupplyResourceNames());

        player.Gold.Should().Be(goldBefore + 55 + 110);
    }

    [Fact]
    public void WorldObject_ToDto_SuppliedMall_SendsHarvestStateWithStaticState()
    {
        var faker = new Faker();
        var (world, mall) = SetupSupplied(faker, "test_supply_mall", "test_mall_shop_a", "test_mall_shop_b");
        mall.UpdateVisits(5);

        var dto = mall.ToDto(world.Objects);

        dto.State.Should().Be(WorldObjectState.Static.ToDescriptionString());
        var harvestState = dto.MechanicData["harvestState"].Should().BeOfType<ASObject>().Subject;
        harvestState["customers"].Should().Be(5);
        harvestState["customersReq"].Should().Be(30);
        harvestState["resources"].Should().BeAssignableTo<List<object>>().Which.Should().Equal("test_mall_shop_a", "test_mall_shop_b");
    }

    [Fact]
    public void WorldObject_ToDto_OpenMallWithoutResources_SendsStaticStateWithoutHarvestState()
    {
        var faker = new Faker();
        var mall = faker.WorldObject(itemName: "test_supply_mall", className: BuildingClassType.Mall, state: WorldObjectState.Open);

        var dto = mall.ToDto();

        dto.State.Should().Be(WorldObjectState.Static.ToDescriptionString());
        dto.MechanicData.Should().NotContainKey("harvestState");
    }

    [Fact]
    public void Player_HandleQuestsProgress_MallSupplyActions_FollowClientCounting()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: "qm_test_mall_supply", length: 4);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("openBusinessByClass", className: nameof(BuildingClassType.Mall));
        player.HandleQuestsProgress("openBusinessByKeyword", itemName: "test_supply_mall");
        player.HandleQuestsProgress("countSupplyItemByName", itemName: "test_supply_mall");

        quest.Progress.Should().Equal(1, 1, 1, 0);
    }
}
