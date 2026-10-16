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
public class TutorialTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task Tutorial_ValidBuilding_IncrementsTutorialCounter()
    {
        var business = Faker.WorldObject(itemName: "test_tutorial_social_business", className: BuildingClassType.Business, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [business]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Tutorial(Context);
        var request = new TutorialRequest { ObjectId = 5, GameMode = "supply" };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var building = await Context.Set<WorldObject>().Include(x => x.MechanicCounters).FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);
        building.MechanicCounters.Should().ContainSingle(x => x.MechanicType == "tutorial").Which.Count.Should().Be(1);
    }

    [Fact]
    public async Task Tutorial_CalledTwice_IncrementsTutorialCounterTwice()
    {
        var business = Faker.WorldObject(itemName: "test_tutorial_social_business", className: BuildingClassType.Business, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [business]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Tutorial(Context);
        var request = new TutorialRequest { ObjectId = 5, GameMode = "supply" };

        await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);
        await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        var building = await Context.Set<WorldObject>().Include(x => x.MechanicCounters).FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);
        building.MechanicCounters.Should().ContainSingle(x => x.MechanicType == "tutorial").Which.Count.Should().Be(2);
    }

    [Fact]
    public async Task Tutorial_BuildingReferencedByTempId_IncrementsTutorialCounter()
    {
        var business = Faker.WorldObject(itemName: "test_tutorial_social_business", className: BuildingClassType.Business, worldFlatId: 5, tempId: 16777230);
        var player = Faker.Player(world: Faker.World(objects: [business]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Tutorial(Context);
        var request = new TutorialRequest { ObjectId = 16777230, GameMode = "supply" };

        await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        var building = await Context.Set<WorldObject>().Include(x => x.MechanicCounters).FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);
        building.MechanicCounters.Should().ContainSingle(x => x.MechanicType == "tutorial").Which.Count.Should().Be(1);
    }

    [Fact]
    public async Task Tutorial_ItemWithoutTutorialMechanic_ThrowsException()
    {
        var hotel = Faker.WorldObject(itemName: "test_named_hotel", className: BuildingClassType.Business, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [hotel]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Tutorial(Context);
        var request = new TutorialRequest { ObjectId = 5, GameMode = "supply" };

        var act = () => handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();

        var building = await Context.Set<WorldObject>().Include(x => x.MechanicCounters).FirstAsync(x => x.Id == hotel.Id, TestContext.Current.CancellationToken);
        building.MechanicCounters.Should().BeEmpty();
    }

    [Fact]
    public async Task Tutorial_WrongGameMode_ThrowsException()
    {
        var business = Faker.WorldObject(itemName: "test_tutorial_social_business", className: BuildingClassType.Business, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [business]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Tutorial(Context);
        var request = new TutorialRequest { ObjectId = 5, GameMode = "GMPlay" };

        var act = () => handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Tutorial_UnknownObject_ThrowsException()
    {
        var player = Faker.Player(world: Faker.World(objects: []));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Tutorial(Context);
        var request = new TutorialRequest { ObjectId = 5, GameMode = "supply" };

        var act = () => handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Tutorial_InvalidUser_ThrowsException()
    {
        var handler = new Tutorial(Context);
        var request = new TutorialRequest { ObjectId = 5, GameMode = "supply" };

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }
}
