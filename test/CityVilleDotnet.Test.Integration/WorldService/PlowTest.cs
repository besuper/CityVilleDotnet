using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.WorldService;

// plot_strawberries withers after 0.001 * 82800s * witherMultiplier (3) = 248.4s
[Collection("Database")]
public class PlowTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private static PlowRequest CreatePlowRequest(int x, int y)
    {
        return new PlowRequest
        {
            Building = new BuildingPlowRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task Plow_GrownPlotNotWithered_ThrowsInvalidState()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        time.Advance(TimeSpan.FromSeconds(100));

        var handler = new Plow(Context);

        var act = () => handler.HandlePacket(CreatePlowRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public async Task Plow_WitheredPlot_PlowsAndClearsContract()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        time.Advance(TimeSpan.FromSeconds(300));

        var handler = new Plow(Context);

        var response = await handler.HandlePacket(CreatePlowRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var updatedPlot = await Context.Set<WorldObject>().FirstAsync(x => x.Id == plot.Id, TestContext.Current.CancellationToken);

        updatedPlot.State.Should().Be(WorldObjectState.Plowed);
        updatedPlot.ContractName.Should().BeNull();
    }
}
