using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class LinkedObjectMoveCompactTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task LinkedObjectMoveCompact_MacroObjectGroup_MovesEveryObjectAndKeepsZ()
    {
        var house = Faker.WorldObject(itemName: "test_island_house", className: BuildingClassType.Residence, x: 16, y: 13, direction: 0, worldFlatId: 1);
        var background = Faker.WorldObject(itemName: "test_island_bg", className: BuildingClassType.BasePier, x: 10, y: 10, z: 1, direction: 0, worldFlatId: 2);
        var tower = Faker.WorldObject(itemName: "test_island_tower", className: BuildingClassType.Municipal, x: 10, y: 17, direction: 0, worldFlatId: 3);
        var world = Faker.World(objects: [house, background, tower]);
        world.CreateMacroObject("test_island_macro", "test_island_macro", [house, background, tower]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new LinkedObjectMoveCompact(Context);
        var request = CreateRequest(
            new LinkedObjectRequest { Id = 1, X = 26, Y = 3, Direction = 0 },
            new LinkedObjectRequest { Id = 2, X = 20, Y = 0, Direction = 0 },
            new LinkedObjectRequest { Id = 3, X = 20, Y = 7, Direction = 0 });

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        house.X.Should().Be(26);
        house.Y.Should().Be(3);
        background.X.Should().Be(20);
        background.Y.Should().Be(0);
        background.Z.Should().Be(1);
        tower.X.Should().Be(20);
        tower.Y.Should().Be(7);
        world.MacroObjects.Should().ContainSingle();
    }

    [Fact]
    public async Task LinkedObjectMoveCompact_ObjectWithTempId_UpdatesDirection()
    {
        var house = Faker.WorldObject(itemName: "test_island_house", className: BuildingClassType.Residence, tempId: 5000, x: 16, y: 13, direction: 0, worldFlatId: 1);
        var world = Faker.World(objects: [house]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new LinkedObjectMoveCompact(Context);
        var request = CreateRequest(new LinkedObjectRequest { Id = 5000, X = 4, Y = 8, Direction = 2 });

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        house.X.Should().Be(4);
        house.Y.Should().Be(8);
        house.Direction.Should().Be(2);
    }

    [Fact]
    public async Task LinkedObjectMoveCompact_OnlyParentCountsForMoveQuest()
    {
        var house = Faker.WorldObject(itemName: "test_island_house", className: BuildingClassType.Residence, x: 16, y: 13, worldFlatId: 1);
        var tower = Faker.WorldObject(itemName: "test_island_tower", className: BuildingClassType.Municipal, x: 10, y: 17, worldFlatId: 2);
        var world = Faker.World(objects: [house, tower]);
        var player = Faker.Player(world: world);
        var quest = Faker.Quest(name: "qm_test_move_island", length: 1);
        player.Quests.Add(quest);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new LinkedObjectMoveCompact(Context);
        var request = CreateRequest(
            new LinkedObjectRequest { Id = 1, X = 26, Y = 3, Direction = 0 },
            new LinkedObjectRequest { Id = 2, X = 20, Y = 7, Direction = 0 });

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        quest.Progress[0].Should().Be(1);
    }

    [Fact]
    public async Task LinkedObjectMoveCompact_UnknownObject_ThrowsForceReload()
    {
        var house = Faker.WorldObject(itemName: "test_island_house", className: BuildingClassType.Residence, worldFlatId: 1);
        var world = Faker.World(objects: [house]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new LinkedObjectMoveCompact(Context);
        var request = CreateRequest(
            new LinkedObjectRequest { Id = 1, X = 26, Y = 3, Direction = 0 },
            new LinkedObjectRequest { Id = 99, X = 20, Y = 7, Direction = 0 });

        var act = () => handler.HandlePacket(request, player.Id, CancellationToken.None);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
    }

    private static LinkedObjectMoveCompactRequest CreateRequest(params LinkedObjectRequest[] linkedObjects)
    {
        return new LinkedObjectMoveCompactRequest
        {
            Params = [new LinkedObjectMoveCompactParamsRequest { LinkedObjects = linkedObjects }]
        };
    }
}
