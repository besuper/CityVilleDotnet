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
public class AutoOpenBusinessTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static OpenBusinessRequest CreateRequest(int id)
    {
        return new OpenBusinessRequest
        {
            Building = new BuildingOpenBusiness { Id = id }
        };
    }

    [Fact]
    public async Task AutoOpenBusiness_ClosedBusiness_ConsumesGoodsAndOpens()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed);
        var player = Faker.Player(world: Faker.World(objects: [business]));
        player.SetGoods(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await new AutoOpenBusiness(Context).HandlePacket(CreateRequest(business.WorldFlatId), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var updatedBusiness = await Context.Set<WorldObject>().FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);

        updatedBusiness.State.Should().Be(WorldObjectState.Open);
        player.Goods.Should().Be(85);
    }

    [Fact]
    public async Task AutoOpenBusiness_NotEnoughGoods_ThrowsNotEnoughMoney()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed);
        var player = Faker.Player(world: Faker.World(objects: [business]));
        player.SetGoods(14);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var act = () => new AutoOpenBusiness(Context).HandlePacket(CreateRequest(business.WorldFlatId), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
    }
}
