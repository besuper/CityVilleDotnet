using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

[Collection("Database")]
public class NameTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task Name_ValidName_RenamesBuilding()
    {
        var hotel = Faker.WorldObject(itemName: "test_named_hotel", className: BuildingClassType.Business, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [hotel]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Name(Context);
        var request = CreateRequest(5, "OnBuildComplete", "My Hotel");

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var building = await Context.Set<WorldObject>().FirstAsync(x => x.Id == hotel.Id, TestContext.Current.CancellationToken);
        building.CustomName.Should().Be("My Hotel");
    }

    [Fact]
    public async Task Name_BuildingReferencedByTempId_RenamesBuilding()
    {
        var hotel = Faker.WorldObject(itemName: "test_named_hotel", className: BuildingClassType.Business, worldFlatId: 5, tempId: 16777230);
        var player = Faker.Player(world: Faker.World(objects: [hotel]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Name(Context);
        var request = CreateRequest(16777230, "OnBuildComplete", "My Hotel");

        await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        var building = await Context.Set<WorldObject>().FirstAsync(x => x.Id == hotel.Id, TestContext.Current.CancellationToken);
        building.CustomName.Should().Be("My Hotel");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("This hotel name is way too long")]
    public async Task Name_InvalidName_ThrowsForceReload(string name)
    {
        var handler = new Name(Context);
        var request = CreateRequest(5, "OnBuildComplete", name);

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
    }

    [Fact]
    public async Task Name_MissingName_ThrowsForceReload()
    {
        var handler = new Name(Context);
        var request = new NameRequest
        {
            ObjectId = 5,
            GameMode = "OnBuildComplete",
            ExtraData = new Dictionary<string, object> { ["operation"] = "name" }
        };

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
    }

    [Fact]
    public async Task Name_ItemWithoutNameMechanic_ThrowsException()
    {
        var marina = Faker.WorldObject(itemName: "test_marina", className: BuildingClassType.Decoration, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [marina]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Name(Context);
        var request = CreateRequest(5, "OnBuildComplete", "My Marina");

        var act = () => handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();

        var building = await Context.Set<WorldObject>().FirstAsync(x => x.Id == marina.Id, TestContext.Current.CancellationToken);
        building.CustomName.Should().BeNull();
    }

    [Fact]
    public async Task Name_WrongGameMode_ThrowsException()
    {
        var hotel = Faker.WorldObject(itemName: "test_named_hotel", className: BuildingClassType.Business, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [hotel]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Name(Context);
        var request = CreateRequest(5, "GMPlay", "My Hotel");

        var act = () => handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Name_InvalidUser_ThrowsException()
    {
        var handler = new Name(Context);
        var request = CreateRequest(5, "OnBuildComplete", "My Hotel");

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    private static NameRequest CreateRequest(int objectId, string gameMode, string name) => new()
    {
        ObjectId = objectId,
        GameMode = gameMode,
        ExtraData = new Dictionary<string, object>
        {
            ["operation"] = "name",
            ["name"] = name
        }
    };
}
