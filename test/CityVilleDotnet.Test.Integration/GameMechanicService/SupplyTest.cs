using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

// See test_supply_mall and test_mall_shop_* in TestData/gameSettings.xml (shop a: 10 goods, shop b: 20 premium goods)
[Collection("Database")]
public class SupplyTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int MallId = 1;

    private async Task<(Player Player, WorldObject Mall)> SeedMall(int goods, int premiumGoods, Quest? quest = null)
    {
        var mall = Faker.WorldObject(itemName: "test_supply_mall", className: BuildingClassType.Mall, x: 1, y: 1, worldFlatId: MallId);
        var shopA = Faker.WorldObject(itemName: "test_mall_shop_a", className: BuildingClassType.Business, worldFlatId: 10);
        var shopB = Faker.WorldObject(itemName: "test_mall_shop_b", className: BuildingClassType.Business, worldFlatId: 11);
        shopA.StoreIn(MallId, 0);
        shopB.StoreIn(MallId, 1);
        var player = Faker.Player(world: Faker.World(objects: [mall, shopA, shopB]));
        player.SetGoods(goods);
        player.SetPremiumGoods(premiumGoods);

        if (quest is not null)
        {
            player.Quests.Add(quest);
        }

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        return (player, mall);
    }

    private Task<ASObject> SupplyMall(Player player)
    {
        return new Supply(Context).HandlePacket(new SupplyRequest { ObjectId = MallId }, player.Id, TestContext.Current.CancellationToken);
    }

    private async Task<Player> LoadPlayer(Player player)
    {
        Context.ChangeTracker.Clear();

        return await Context.Set<Player>().AsNoTracking().Include(x => x.Quests).SingleAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
    }

    private async Task<WorldObject> LoadMall(WorldObject mall)
    {
        Context.ChangeTracker.Clear();

        return await Context.Set<WorldObject>().AsNoTracking().Include(x => x.SupplyResources).SingleAsync(x => x.Id == mall.Id, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task Supply_Mall_PaysEachCommodityAndPersistsSuppliedShops()
    {
        var (player, mall) = await SeedMall(goods: 100, premiumGoods: 100);

        var response = await SupplyMall(player);

        response["errorType"].Should().Be(0);
        var storedPlayer = await LoadPlayer(player);
        storedPlayer.Goods.Should().Be(90);
        storedPlayer.PremiumGoods.Should().Be(80);
        var storedMall = await LoadMall(mall);
        storedMall.GetSupplyResourceNames().Should().Equal("test_mall_shop_a", "test_mall_shop_b");
        storedMall.State.Should().Be(WorldObjectState.Open);
    }

    [Fact]
    public async Task Supply_Mall_ProgressesSupplyQuestTasks()
    {
        var (player, _) = await SeedMall(goods: 100, premiumGoods: 100, quest: Faker.Quest(name: "qm_test_mall_supply", length: 4));

        await SupplyMall(player);

        var storedPlayer = await LoadPlayer(player);
        storedPlayer.Quests.Single(x => x.Name == "qm_test_mall_supply").Progress.Should().Equal(1, 1, 1, 0);
    }

    [Fact]
    public async Task Supply_MallAlreadySupplied_ThrowsWithoutPayingTwice()
    {
        var (player, _) = await SeedMall(goods: 100, premiumGoods: 100);
        await SupplyMall(player);
        Context.ChangeTracker.Clear();

        var act = () => SupplyMall(player);

        await act.Should().ThrowAsync<Exception>();
        var storedPlayer = await LoadPlayer(player);
        storedPlayer.Goods.Should().Be(90);
        storedPlayer.PremiumGoods.Should().Be(80);
    }

    [Fact]
    public async Task Supply_Mall_NotEnoughPremiumGoods_ThrowsWithoutSupplying()
    {
        var (player, mall) = await SeedMall(goods: 1000, premiumGoods: 5);

        var act = () => SupplyMall(player);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        var storedMall = await LoadMall(mall);
        storedMall.IsStorageSupplied().Should().BeFalse();
    }
}
