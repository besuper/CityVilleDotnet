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

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(45);

        var updatedResidence = await Context.Set<WorldObject>().FirstAsync(x => x.Id == residence.Id);

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

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(48);

        var updatedPlot = await Context.Set<WorldObject>().FirstAsync(x => x.Id == plot.Id);

        updatedPlot.State.Should().Be(WorldObjectState.Grown);
    }

    [Fact]
    public async Task MakeResourceInstantReady_AlmostReady_ChargesMinimumOneCash()
    {
        var residence = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - 410_000, worldFlatId: 5);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetCash(50);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, CancellationToken.None);

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

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new MakeResourceInstantReady(Context, NullLogger<MakeResourceInstantReady>.Instance);

        var response = await handler.HandlePacket(new MakeResourceInstantReadyRequest { BuildingId = 5 }, player.Id, CancellationToken.None);

        response["errorType"].Should().Be((int)GameErrorType.InvalidState);
        player.Cash.Should().Be(50);
    }
}
