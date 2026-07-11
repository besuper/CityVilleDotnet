using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

[Collection("Database")]
public class LootTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task Loot_ZooEnclosure_RemovesCashAndStoresRandomAnimal()
    {
        var enclosure = Faker.WorldObject(itemName: "test_enclosure", className: BuildingClassType.ZooEnclosure, worldFlatId: 5);
        var world = Faker.World(objects: [enclosure]);
        var player = Faker.Player(world: world);
        player.SetCash(100);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new Loot(Context);
        var request = new LootRequest { ObjectId = 5 };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        // GetZooDonationNpcPrice falls back to 20 for a non-standard enclosure
        player.Cash.Should().Be(80);

        var data = response["data"] as ASObject;
        data.Should().NotBeNull();

        var loot = data!["loot"] as string;
        loot.Should().BeOneOf("test_animal_common", "test_animal_uncommon", "test_animal_rare");

        var storedEnclosure = await Context.Set<WorldObject>()
            .Include(x => x.StorageItems)
            .FirstAsync(x => x.WorldFlatId == 5);

        storedEnclosure.StorageItems.Should().ContainSingle(x => x.Name == loot);
    }

    [Fact]
    public async Task Loot_NonEnclosure_ThrowsException()
    {
        var building = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, worldFlatId: 7);
        var world = Faker.World(objects: [building]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new Loot(Context);
        var request = new LootRequest { ObjectId = 7 };

        var act = () => handler.HandlePacket(request, player.Id, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }
}
