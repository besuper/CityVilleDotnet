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

// Item.as clearEnergyCost comes from energyCost@clear (1 for test_tree_energy)
[Collection("Database")]
public class ClearTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task Clear_Wilderness_RemovesEnergyAndObject()
    {
        var tree = Faker.WorldObject(itemName: "test_tree_energy", className: BuildingClassType.Wilderness, worldFlatId: 5);
        var world = Faker.World(objects: [tree]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var energyBefore = player.Energy;
        var handler = new Clear(Context);

        var response = await handler.HandlePacket(new ClearRequest { Building = new BuildingClearRequest { Id = 5 } }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Energy.Should().Be(energyBefore - 1);
        (await Context.Set<WorldObject>().AnyAsync(x => x.Id == tree.Id, TestContext.Current.CancellationToken)).Should().BeFalse();
    }

    [Fact]
    public async Task Clear_ObjectReferencedByTempId_RemovesObject()
    {
        var tree = Faker.WorldObject(itemName: "test_tree_energy", className: BuildingClassType.Wilderness, tempId: 63000, worldFlatId: 5);
        var world = Faker.World(objects: [tree]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Clear(Context);

        var response = await handler.HandlePacket(new ClearRequest { Building = new BuildingClearRequest { Id = 63000 } }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (await Context.Set<WorldObject>().AnyAsync(x => x.Id == tree.Id, TestContext.Current.CancellationToken)).Should().BeFalse();
    }

    [Fact]
    public async Task Clear_NotEnoughEnergy_ThrowsForceReloadAndKeepsObject()
    {
        var tree = Faker.WorldObject(itemName: "test_tree_energy", className: BuildingClassType.Wilderness, worldFlatId: 5);
        var world = Faker.World(objects: [tree]);
        var player = Faker.Player(world: world);
        player.SetEnergy(0);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Clear(Context);

        var act = () => handler.HandlePacket(new ClearRequest { Building = new BuildingClearRequest { Id = 5 } }, player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
        world.Objects.Should().Contain(tree);
    }

    [Fact]
    public async Task Clear_UnknownId_ThrowsException()
    {
        var tree = Faker.WorldObject(itemName: "test_tree_energy", className: BuildingClassType.Wilderness, worldFlatId: 5);
        var world = Faker.World(objects: [tree]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Clear(Context);

        var act = () => handler.HandlePacket(new ClearRequest { Building = new BuildingClearRequest { Id = 99 } }, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>().WithMessage("*99*");
    }
}
