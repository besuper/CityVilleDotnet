using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

// greenhouse_grassroots numCrop 8, plot_shrimp cost 41 withered after 5400s: int(41 * 0.5) * 8 = 160 coins
[Collection("Database")]
public class GreenHouseClearTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task GreenHouseClear_WitheredPlot_RefundsCoinsAndPlows()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var greenHouse = Faker.WorldObject(itemName: "greenhouse_grassroots", className: BuildingClassType.GreenHouse, contractName: "plot_shrimp", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: 42);
        var world = Faker.World(objects: [greenHouse]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        time.Advance(TimeSpan.FromSeconds(5500));

        var goldBefore = player.Gold;
        var handler = new GreenHouseClear(Context);

        var response = await handler.HandlePacket(new GreenHouseClearRequest { ObjectId = 42 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Gold.Should().Be(goldBefore + 160);
        greenHouse.State.Should().Be(WorldObjectState.Plowed);
        greenHouse.ContractName.Should().BeNull();
        greenHouse.PlantTime.Should().BeNull();
    }

    [Fact]
    public async Task GreenHouseClear_PlotNotWithered_ThrowsForceReloadWithoutRefund()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var greenHouse = Faker.WorldObject(itemName: "greenhouse_grassroots", className: BuildingClassType.GreenHouse, contractName: "plot_shrimp", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: 42);
        var world = Faker.World(objects: [greenHouse]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        time.Advance(TimeSpan.FromSeconds(5300));

        var goldBefore = player.Gold;
        var handler = new GreenHouseClear(Context);

        var act = () => handler.HandlePacket(new GreenHouseClearRequest { ObjectId = 42 }, player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
        player.Gold.Should().Be(goldBefore);
        greenHouse.ContractName.Should().Be("plot_shrimp");
        greenHouse.State.Should().Be(WorldObjectState.Planted);
    }

    [Fact]
    public async Task GreenHouseClear_NotAGreenHouse_Throws()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_shrimp", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: 42);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        time.Advance(TimeSpan.FromSeconds(5500));

        var goldBefore = player.Gold;
        var handler = new GreenHouseClear(Context);

        var act = () => handler.HandlePacket(new GreenHouseClearRequest { ObjectId = 42 }, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        player.Gold.Should().Be(goldBefore);
        plot.ContractName.Should().Be("plot_shrimp");
    }
}
