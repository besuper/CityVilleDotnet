using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

[Collection("Database")]
public class HarvestStateTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task HarvestState_BridgeWithPopulationItems_DistributesPopulationToResidences()
    {
        var bridge = Faker.WorldObject(itemName: "test_bridge", className: BuildingClassType.Bridge, x: 10, y: 10, worldFlatId: 5);
        var residence = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, x: 20, y: 20, worldFlatId: 10);
        var world = Faker.World(objects: [bridge, residence]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var startingEnergy = player.Energy;

        var handler = new HarvestState(Context);
        var request = new HarvestStateRequest { ObjectId = 5 };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        player.Energy.Should().Be(startingEnergy - 5);

        var updatedResidence = await Context.Set<WorldObject>()
            .Include(x => x.MechanicCounters)
            .FirstAsync(x => x.Id == residence.Id);

        updatedResidence.GetBonusPopulation().Should().Be(10);
        player.GetWorld().GetCurrentPopulation().Should().Be(10);

        var inventoryItem = await Context.Set<InventoryItem>().FirstOrDefaultAsync(x => x.Name == "test_population_add");
        inventoryItem.Should().BeNull();
    }

    [Fact]
    public async Task HarvestState_NoResidence_PopulationIsNotDistributed()
    {
        var bridge = Faker.WorldObject(itemName: "test_bridge", className: BuildingClassType.Bridge, x: 10, y: 10, worldFlatId: 5);
        var world = Faker.World(objects: [bridge]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new HarvestState(Context);
        var request = new HarvestStateRequest { ObjectId = 5 };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        player.GetWorld().GetCurrentPopulation().Should().Be(0);

        var inventoryItem = await Context.Set<InventoryItem>().FirstOrDefaultAsync(x => x.Name == "test_population_add");
        inventoryItem.Should().BeNull();
    }
}
