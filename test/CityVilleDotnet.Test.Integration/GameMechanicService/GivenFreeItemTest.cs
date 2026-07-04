using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

[Collection("Database")]
public class GivenFreeItemTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task GivenFreeItem_PlacedByClient_CreatesWorldObjectAndMarksOwner()
    {
        var marina = Faker.WorldObject(itemName: "test_marina", className: BuildingClassType.Decoration, x: 10, y: 10, worldFlatId: 5);
        var world = Faker.World(objects: [marina]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new GivenFreeItem(Context, NullLogger<GivenFreeItem>.Instance);
        var request = new GivenFreeItemRequest
        {
            ObjectId = 5,
            GameMode = "load",
            ExtraData = new Dictionary<string, object>
            {
                ["operation"] = "performGiveFreeItem",
                ["tempID"] = new Dictionary<string, object> { ["freeObj"] = 16777220 }
            }
        };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var ferry = await Context.Set<WorldObject>().FirstOrDefaultAsync(x => x.ItemName == "test_ferry");
        ferry.Should().NotBeNull();
        ferry.TempId.Should().Be(16777220);
        ferry.X.Should().Be(7);
        ferry.Y.Should().Be(16);

        var owner = await Context.Set<WorldObject>().FirstAsync(x => x.Id == marina.Id);
        owner.GivenFreeItem.Should().BeTrue();
    }

    [Fact]
    public async Task GivenFreeItem_ClientCollision_AddsItemToInventory()
    {
        var marina = Faker.WorldObject(itemName: "test_marina", className: BuildingClassType.Decoration, x: 10, y: 10, worldFlatId: 5);
        var world = Faker.World(objects: [marina]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new GivenFreeItem(Context, NullLogger<GivenFreeItem>.Instance);
        var request = new GivenFreeItemRequest
        {
            ObjectId = 5,
            GameMode = "load",
            ExtraData = new Dictionary<string, object>
            {
                ["operation"] = "performGiveFreeItem",
                ["tempID"] = new Dictionary<string, object>()
            }
        };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var ferry = await Context.Set<WorldObject>().FirstOrDefaultAsync(x => x.ItemName == "test_ferry");
        ferry.Should().BeNull();

        var inventoryItem = await Context.Set<InventoryItem>().FirstOrDefaultAsync(x => x.Name == "test_ferry");
        inventoryItem.Should().NotBeNull();
        inventoryItem.Amount.Should().Be(1);

        var owner = await Context.Set<WorldObject>().FirstAsync(x => x.Id == marina.Id);
        owner.GivenFreeItem.Should().BeTrue();
    }

    [Fact]
    public async Task GivenFreeItem_AlreadyGiven_DoesNotGiveTwice()
    {
        var marina = Faker.WorldObject(itemName: "test_marina", className: BuildingClassType.Decoration, x: 10, y: 10, worldFlatId: 5);
        marina.MarkFreeItemGiven();
        var world = Faker.World(objects: [marina]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new GivenFreeItem(Context, NullLogger<GivenFreeItem>.Instance);
        var request = new GivenFreeItemRequest
        {
            ObjectId = 5,
            GameMode = "load",
            ExtraData = new Dictionary<string, object>
            {
                ["operation"] = "performGiveFreeItem",
                ["tempID"] = new Dictionary<string, object> { ["freeObj"] = 16777220 }
            }
        };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var ferry = await Context.Set<WorldObject>().FirstOrDefaultAsync(x => x.ItemName == "test_ferry");
        ferry.Should().BeNull();
    }

    [Fact]
    public async Task GivenFreeItem_OwnerReferencedByTempId_IsFound()
    {
        var marina = Faker.WorldObject(itemName: "test_marina", className: BuildingClassType.Decoration, x: 10, y: 10, worldFlatId: 5, tempId: 16777230);
        var world = Faker.World(objects: [marina]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new GivenFreeItem(Context, NullLogger<GivenFreeItem>.Instance);
        var request = new GivenFreeItemRequest
        {
            ObjectId = 16777230,
            GameMode = "load",
            ExtraData = new Dictionary<string, object>
            {
                ["operation"] = "performGiveFreeItem",
                ["tempID"] = new Dictionary<string, object> { ["freeObj"] = 16777220 }
            }
        };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var owner = await Context.Set<WorldObject>().FirstAsync(x => x.Id == marina.Id);
        owner.GivenFreeItem.Should().BeTrue();
    }
}
