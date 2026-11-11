using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class MakeResourceInstantReadyTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task MakeResourceInstantReady_PlantedResidence_ChargesClientCostAndSetsGrown()
    {
        // 23 hours left: 2.0 * 23 ^ 0.25 = 4.38 => 5 cash
        var residence = Faker.WorldObject(itemName: "test_res_slow", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: 5);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetCash(50);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(45);

        var updatedResidence = await Context.Set<WorldObject>().FirstAsync(x => x.Id == residence.Id, TestContext.Current.CancellationToken);

        updatedResidence.State.Should().Be(WorldObjectState.Grown);
    }

    [Fact]
    public async Task MakeResourceInstantReady_PlantedPlot_ChargesClientCostAndSetsGrown()
    {
        // 92 hours left: 0.25 * 92 ^ 0.4 = 1.53 => 2 cash
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "test_crop_slow", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: 5);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);
        player.SetCash(50);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(48);

        var updatedPlot = await Context.Set<WorldObject>().FirstAsync(x => x.Id == plot.Id, TestContext.Current.CancellationToken);

        updatedPlot.State.Should().Be(WorldObjectState.Grown);
    }

    [Fact]
    public async Task MakeResourceInstantReady_AlmostReady_ChargesMinimumOneCash()
    {
        var residence = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - 410_000, worldFlatId: 5);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetCash(50);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(49);
    }

    [Fact]
    public async Task MakeResourceInstantReady_AlreadyGrown_ReturnsInvalidStateWithoutCharging()
    {
        var residence = Faker.WorldObject(itemName: "test_res_slow", className: BuildingClassType.Residence, state: WorldObjectState.Grown, worldFlatId: 5);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetCash(50);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.InvalidState);
        player.Cash.Should().Be(50);
    }

    [Fact]
    public async Task MakeResourceInstantReady_Corporation_ChargesClientCostAndResetsHarvestTimer()
    {
        // 10 minutes left: 2.0 * (1/6) ^ 0.25 = 1.28 => 2 cash
        var corporation = Faker.WorldObject(itemName: "test_corporation", className: BuildingClassType.Corporation, state: WorldObjectState.Planted, worldFlatId: 5);
        corporation.InitHarvestEndTime();
        var world = Faker.World(objects: [corporation]);
        var player = Faker.Player(world: world);
        player.SetCash(50);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(48);

        var updatedCorporation = await Context.Set<WorldObject>().FirstAsync(x => x.Id == corporation.Id, TestContext.Current.CancellationToken);

        updatedCorporation.State.Should().Be(WorldObjectState.Planted);
        updatedCorporation.HarvestEndTime.Should().BeCloseTo(ServerUtils.GetCurrentUtcDateTime(), TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task MakeResourceInstantReady_Skyscraper_ChargesClientCostAndResetsHarvestTimer()
    {
        // 23 hours left: 2.0 * 23 ^ 0.25 = 4.38 => 5 cash
        var skyscraper = Faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: 5);
        skyscraper.InitHarvestEndTime();
        var world = Faker.World(objects: [skyscraper]);
        var player = Faker.Player(world: world);
        player.SetCash(50);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(45);

        var updatedSkyscraper = await Context.Set<WorldObject>().FirstAsync(x => x.Id == skyscraper.Id, TestContext.Current.CancellationToken);

        updatedSkyscraper.State.Should().Be(WorldObjectState.Planted);
        updatedSkyscraper.HarvestEndTime.Should().BeCloseTo(ServerUtils.GetCurrentUtcDateTime(), TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task MakeResourceInstantReady_LaunchPad_ChargesFixedCostAndSetsGrown()
    {
        var launchPad = Faker.WorldObject(itemName: "test_launchpad", className: BuildingClassType.LaunchPad, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), worldFlatId: 5);
        var world = Faker.World(objects: [launchPad]);
        var player = Faker.Player(world: world);
        player.SetCash(50);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(47);

        var updatedLaunchPad = await Context.Set<WorldObject>().FirstAsync(x => x.Id == launchPad.Id, TestContext.Current.CancellationToken);

        updatedLaunchPad.State.Should().Be(WorldObjectState.Grown);
    }
}
