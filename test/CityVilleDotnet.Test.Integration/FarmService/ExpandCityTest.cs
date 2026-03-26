using AwesomeAssertions;
using CityVilleDotnet.Api.Services.FarmService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.InventoryItem;
using CityVilleDotnet.Factory.MapRect;
using CityVilleDotnet.Factory.User;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.FarmService;

[Collection("Database")]
public class ExpandCityTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const string ExpansionItemName = "expansion_18x18";
    private const string TreeItemName = "tree_oak";

    [Fact]
    public async Task ExpandCity_ValidRequest()
    {
        var permit = Faker.InventoryItem(itemName: "permits", amount: 5);
        var world = Faker.World();
        var user = Faker.User(world: world);
        user.Player!.InventoryItems.Add(permit);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new ExpandCity(Context);
        var request = new ExpandCityRequest
        {
            ItemName = ExpansionItemName,
            Coordinates = new ExpandCityCoordinates { X = 40, Y = 40 },
            Trees =
            [
                new ExpandCityTree { Id = 1, ItemName = TreeItemName, X = 42, Y = 42 },
                new ExpandCityTree { Id = 2, ItemName = TreeItemName, X = 44, Y = 44 }
            ]
        };

        var response = await handler.HandlePacket(request, user.UserId, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var data = response["data"] as List<object>;
        data.Should().NotBeNull();
        data.Should().HaveCount(2);

        var mapRect = await Context.Set<MapRect>().FirstOrDefaultAsync(m => m.X == 40 && m.Y == 40);

        mapRect.Should().NotBeNull();
        mapRect.Width.Should().Be(18);
        mapRect.Height.Should().Be(18);

        var trees = await Context.Set<WorldObject>()
            .Where(o => o.ItemName == TreeItemName)
            .ToListAsync();

        trees.Should().HaveCount(2);

        var updatedPlayer = await Context.Set<User>()
            .Where(u => u.UserId == user.UserId)
            .Select(u => u.Player)
            .FirstAsync();

        updatedPlayer!.ExpansionsPurchased.Should().Be(1);

        var updatedPermit = await Context.Set<InventoryItem>().FirstOrDefaultAsync(i => i.Name == "permits");

        // First expansion requires 1 permit, so 5 - 1 = 4
        updatedPermit.Should().NotBeNull();
        updatedPermit!.Amount.Should().Be(4);
    }

    [Fact]
    public async Task ExpandCity_NoTrees()
    {
        var permit = Faker.InventoryItem(itemName: "permits", amount: 5);
        var world = Faker.World();
        var user = Faker.User(world: world);
        user.Player!.InventoryItems.Add(permit);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new ExpandCity(Context);
        var request = new ExpandCityRequest
        {
            ItemName = ExpansionItemName,
            Coordinates = new ExpandCityCoordinates { X = 40, Y = 40 },
            Trees = []
        };

        var response = await handler.HandlePacket(request, user.UserId, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var mapRect = await Context.Set<MapRect>().FirstOrDefaultAsync(m => m.X == 40 && m.Y == 40);

        mapRect.Should().NotBeNull();

        var trees = await Context.Set<WorldObject>()
            .Where(o => o.ItemName == TreeItemName)
            .ToListAsync();

        trees.Should().BeEmpty();
    }

    [Fact]
    public async Task ExpandCity_NotEnoughPermits_ThrowsException()
    {
        var world = Faker.World();
        var user = Faker.User(world: world);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new ExpandCity(Context);
        var request = new ExpandCityRequest
        {
            ItemName = ExpansionItemName,
            Coordinates = new ExpandCityCoordinates { X = 40, Y = 40 },
            Trees = []
        };

        var act = () => handler.HandlePacket(request, user.UserId, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage("*permits*");
    }

    [Fact]
    public async Task ExpandCity_MapRectAlreadyExists_ThrowsException()
    {
        var existingMapRect = Faker.MapRect(x: 40, y: 40, width: 18, height: 18);
        var permit = Faker.InventoryItem(itemName: "permits", amount: 5);
        var world = Faker.World(mapRects: [existingMapRect]);
        var user = Faker.User(world: world);
        user.Player!.InventoryItems.Add(permit);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new ExpandCity(Context);
        var request = new ExpandCityRequest
        {
            ItemName = ExpansionItemName,
            Coordinates = new ExpandCityCoordinates { X = 40, Y = 40 },
            Trees = []
        };

        var act = () => handler.HandlePacket(request, user.UserId, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage("*already exist*");
    }

    [Fact]
    public async Task ExpandCity_NotExistingItem_ThrowsException()
    {
        var permit = Faker.InventoryItem(itemName: "permits", amount: 5);
        var world = Faker.World();
        var user = Faker.User(world: world);
        user.Player!.InventoryItems.Add(permit);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new ExpandCity(Context);
        var request = new ExpandCityRequest
        {
            ItemName = "unknown",
            Coordinates = new ExpandCityCoordinates { X = 40, Y = 40 },
            Trees = []
        };

        var act = () => handler.HandlePacket(request, user.UserId, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage("*Can't find item*");
    }
}