using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.WorldService;

// Plot.as makeDoobers on a withered plot: int(contract cost * witherRefundMultiplier) coins
// plot_strawberries: cost 15 * 0.5 = 7 coins, withered after 248.4s
[Collection("Database")]
public class ClearWitheredTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private static ClearWitheredRequest CreateClearWitheredRequest(int x, int y)
    {
        return new ClearWitheredRequest
        {
            Building = new BuildingClearWitheredRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task ClearWithered_WitheredPlot_RefundsCoinsAndPlows()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        time.Advance(TimeSpan.FromSeconds(300));

        var goldBefore = player.Gold;
        var handler = new ClearWithered(Context);

        var response = await handler.HandlePacket(CreateClearWitheredRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Gold.Should().Be(goldBefore + 7);
        plot.State.Should().Be(WorldObjectState.Plowed);
        plot.ContractName.Should().BeNull();
    }

    [Fact]
    public async Task ClearWithered_PlotNotWithered_ThrowsInvalidStateWithoutRefund()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        time.Advance(TimeSpan.FromSeconds(100));

        var goldBefore = player.Gold;
        var handler = new ClearWithered(Context);

        var act = () => handler.HandlePacket(CreateClearWitheredRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidState);
        player.Gold.Should().Be(goldBefore);
        plot.ContractName.Should().Be("plot_strawberries");
    }
}
