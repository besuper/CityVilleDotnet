using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class SellTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task Sell_ByWorldFlatId_RemovesOnlyThatBuildingAndGivesSellPrice()
    {
        var keptTree = Faker.WorldObject(itemName: "deco_tree", className: BuildingClassType.Decoration, x: 10, y: 10, worldFlatId: 5);
        var soldTree = Faker.WorldObject(itemName: "deco_tree", className: BuildingClassType.Decoration, x: 11, y: 10, worldFlatId: 6);
        var world = Faker.World(objects: [keptTree, soldTree]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var handler = new Sell(Context);
        var request = new SellRequest { Building = new SellBuildingRequest { Id = 6 } };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var remaining = await Context.Set<WorldObject>().Where(x => x.Id == keptTree.Id || x.Id == soldTree.Id).ToListAsync(TestContext.Current.CancellationToken);

        remaining.Should().ContainSingle().Which.WorldFlatId.Should().Be(5);
        player.Gold.Should().Be(goldBefore + 3);
    }

    [Fact]
    public async Task Sell_GoodsItem_UsesGoodsSellPrice()
    {
        var shop = Faker.WorldObject(itemName: "test_island_shop", className: BuildingClassType.TimedBusiness, x: 10, y: 10, worldFlatId: 5);
        var world = Faker.World(objects: [shop]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var handler = new Sell(Context);
        var request = new SellRequest { Building = new SellBuildingRequest { Id = 5 } };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Gold.Should().Be(goldBefore + 30);
    }

    [Fact]
    public async Task Sell_UnknownId_ThrowsException()
    {
        var tree = Faker.WorldObject(itemName: "deco_tree", className: BuildingClassType.Decoration, x: 10, y: 10, worldFlatId: 5);
        var world = Faker.World(objects: [tree]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var handler = new Sell(Context);
        var request = new SellRequest { Building = new SellBuildingRequest { Id = 99 } };

        var act = () => handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
        player.Gold.Should().Be(goldBefore);
    }
}
