using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;

namespace CityVilleDotnet.Test.Integration.WorldService;

// calculateCashCostPerUpgradeAction: upgrade_actions * businessUpgradeCost (1) - actions already done
[Collection("Database")]
public class BuyUpgradeActionTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static BuyUpgradeActionRequest CreateBuyUpgradeActionRequest(int x, int y)
    {
        return new BuyUpgradeActionRequest
        {
            Building = new BuyUpgradeActionBuildingRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task BuyUpgradeAction_PartialProgress_ChargesRemainingActionsAndFillsCount()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        business.SetUpgradeAction(2);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);
        player.SetCash(10);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyUpgradeAction(Context);

        var response = await handler.HandlePacket(CreateBuyUpgradeActionRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(7);
        business.UpgradeActionCount.Should().Be(5);
    }

    [Fact]
    public async Task BuyUpgradeAction_AlreadyComplete_ChargesNothing()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        business.SetUpgradeAction(7);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);
        player.SetCash(10);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyUpgradeAction(Context);

        var response = await handler.HandlePacket(CreateBuyUpgradeActionRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(10);
    }

    [Fact]
    public async Task BuyUpgradeAction_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);
        player.SetCash(4);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyUpgradeAction(Context);

        var act = () => handler.HandlePacket(CreateBuyUpgradeActionRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        player.Cash.Should().Be(4);
    }
}
