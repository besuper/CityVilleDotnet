using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
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
public class OpenBusinessTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static OpenBusinessRequest CreateOpenBusinessRequest(int x, int y)
    {
        return new OpenBusinessRequest
        {
            Building = new BuildingOpenBusiness
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task OpenBusiness_ClosedBusiness_ConsumesGoodsWithoutEnergyCost()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);
        player.SetGoods(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var energyBefore = player.Energy;
        var handler = new OpenBusiness(Context);

        var response = await handler.HandlePacket(CreateOpenBusinessRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var updatedBusiness = await Context.Set<WorldObject>().FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);

        updatedBusiness.State.Should().Be(WorldObjectState.Open);
        updatedBusiness.Visits.Should().Be(0);
        player.Goods.Should().Be(85);
        player.Energy.Should().Be(energyBefore);
    }

    [Fact]
    public async Task OpenBusiness_NotEnoughGoods_ThrowsNotEnoughMoney()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);
        player.SetGoods(14);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new OpenBusiness(Context);

        var act = () => handler.HandlePacket(CreateOpenBusinessRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
    }
}
