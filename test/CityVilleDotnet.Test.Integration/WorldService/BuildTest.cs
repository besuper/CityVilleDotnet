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

namespace CityVilleDotnet.Test.Integration.WorldService;

// ConstructionSite.as getBuildCost: energyStart for the first build, then energyCostPerBuild (both 1 for construction_3x3_2stage)
[Collection("Database")]
public class BuildTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static BuildRequest CreateBuildRequest(int x, int y)
    {
        return new BuildRequest
        {
            Building = new BuildBuildingRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    private WorldObject CreateConstructionSite()
    {
        var site = Faker.WorldObject(itemName: "bus_bakery", className: BuildingClassType.Business, x: 10, y: 10);
        site.SetAsConstructionSite("construction_3x3_2stage", 2);

        return site;
    }

    [Fact]
    public async Task Build_FirstStage_RemovesEnergyAndAdvancesStage()
    {
        var site = CreateConstructionSite();
        var world = Faker.World(objects: [site]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var energyBefore = player.Energy;
        var handler = new Build(Context);

        var response = await handler.HandlePacket(CreateBuildRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Energy.Should().Be(energyBefore - 1);
        site.Stage.Should().Be(1);
        site.FinishedBuilds.Should().Be(1);
        site.CurrentState.Should().Be(ConstructionState.Idle);
    }

    [Fact]
    public async Task Build_LastStage_ReachesGate()
    {
        var site = CreateConstructionSite();
        var world = Faker.World(objects: [site]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var energyBefore = player.Energy;
        var handler = new Build(Context);

        await handler.HandlePacket(CreateBuildRequest(10, 10), player.Id, TestContext.Current.CancellationToken);
        var response = await handler.HandlePacket(CreateBuildRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Energy.Should().Be(energyBefore - 2);
        site.Stage.Should().Be(2);
        site.IsConstructionComplete().Should().BeTrue();
        site.CurrentState.Should().Be(ConstructionState.AtGate);
    }

    [Fact]
    public async Task Build_NotAConstructionSite_ReturnsForceReload()
    {
        var building = Faker.WorldObject(itemName: "bus_bakery", className: BuildingClassType.Business, x: 10, y: 10);
        var world = Faker.World(objects: [building]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var energyBefore = player.Energy;
        var handler = new Build(Context);

        var response = await handler.HandlePacket(CreateBuildRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.ForceReload);
        player.Energy.Should().Be(energyBefore);
    }

    [Fact]
    public async Task Build_NotEnoughEnergy_ThrowsForceReloadAndKeepsStage()
    {
        var site = CreateConstructionSite();
        var world = Faker.World(objects: [site]);
        var player = Faker.Player(world: world);
        player.SetEnergy(0);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Build(Context);

        var act = () => handler.HandlePacket(CreateBuildRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
        site.Stage.Should().Be(0);
    }
}
