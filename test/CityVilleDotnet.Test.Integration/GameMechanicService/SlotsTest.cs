using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
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
public class SlotsTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int ThemeParkId = 5;
    private const int AttractionId = 6;

    [Fact]
    public async Task Slots_BuildInPlace_InplaceSlot_FillsSlotAndConsumesGateMaterials()
    {
        var themePark = Faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: ThemeParkId);
        var player = Faker.Player(world: Faker.World(objects: [themePark]));
        player.AddItem("test_gate_material", 2);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await new Slots(Context).HandlePacket(CreateRequest("buildInPlace", slotIndex: 1), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        themePark.Slots.Should().ContainSingle(x => x.SlotIndex == 1 && x.ItemName == "test_inplace_attraction");
        player.HasItem("test_gate_material").Should().BeFalse();
    }

    [Fact]
    public async Task Slots_BuildInPlace_NotEnoughGateMaterials_ThrowsException()
    {
        var themePark = Faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: ThemeParkId);
        var player = Faker.Player(world: Faker.World(objects: [themePark]));
        player.AddItem("test_gate_material", 1);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var act = () => new Slots(Context).HandlePacket(CreateRequest("buildInPlace", slotIndex: 1), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Slots_BuildInPlace_StorageSlot_ThrowsException()
    {
        var themePark = Faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: ThemeParkId);
        var player = Faker.Player(world: Faker.World(objects: [themePark]));
        player.AddItem("test_gate_material", 2);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var act = () => new Slots(Context).HandlePacket(CreateRequest("buildInPlace", slotIndex: 0), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Slots_StoreObject_MatchingAttraction_FillsSlotAndRemovesObject()
    {
        var themePark = Faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: ThemeParkId);
        var attraction = Faker.WorldObject(itemName: "test_attraction", className: BuildingClassType.Attraction, worldFlatId: AttractionId);
        var player = Faker.Player(world: Faker.World(objects: [themePark, attraction]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await new Slots(Context).HandlePacket(CreateRequest("storeObject", slotIndex: 0, objectId: AttractionId), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        themePark.Slots.Should().ContainSingle(x => x.SlotIndex == 0 && x.ItemName == "test_attraction");

        var attractionExists = await Context.Set<WorldObject>().AnyAsync(x => x.ItemName == "test_attraction", TestContext.Current.CancellationToken);
        attractionExists.Should().BeFalse();
    }

    [Fact]
    public async Task Slots_StoreObject_WrongItem_ThrowsException()
    {
        var themePark = Faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: ThemeParkId);
        var building = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, worldFlatId: AttractionId);
        var player = Faker.Player(world: Faker.World(objects: [themePark, building]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var act = () => new Slots(Context).HandlePacket(CreateRequest("storeObject", slotIndex: 0, objectId: AttractionId), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Slots_StoreObject_SlotAlreadyFilled_ThrowsException()
    {
        var themePark = Faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: ThemeParkId);
        themePark.FillSlot(0, "test_attraction", 2);
        var attraction = Faker.WorldObject(itemName: "test_attraction", className: BuildingClassType.Attraction, worldFlatId: AttractionId);
        var player = Faker.Player(world: Faker.World(objects: [themePark, attraction]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var act = () => new Slots(Context).HandlePacket(CreateRequest("storeObject", slotIndex: 0, objectId: AttractionId), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Slots_PlaceObjectFromStorage_FilledSlot_EmptiesSlotAndAddsObject()
    {
        var themePark = Faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: ThemeParkId);
        themePark.FillSlot(0, "test_attraction", 2);
        var player = Faker.Player(world: Faker.World(objects: [themePark]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var request = CreateRequest("placeObjectFromStorage", slotIndex: 0, objectData: CreateObjectData("test_attraction"));

        var response = await new Slots(Context).HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        themePark.Slots.Should().BeEmpty();

        var placed = await Context.Set<WorldObject>().SingleAsync(x => x.ItemName == "test_attraction", TestContext.Current.CancellationToken);
        placed.X.Should().Be(12);
        placed.Y.Should().Be(14);
        placed.Direction.Should().Be(1);

        var data = response["data"] as ASObject;
        data.Should().NotBeNull();
        data["objectId"].Should().Be(placed.WorldFlatId);
    }

    [Fact]
    public async Task Slots_PlaceObjectFromStorage_EmptySlot_ThrowsException()
    {
        var themePark = Faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: ThemeParkId);
        var player = Faker.Player(world: Faker.World(objects: [themePark]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var request = CreateRequest("placeObjectFromStorage", slotIndex: 0, objectData: CreateObjectData("test_attraction"));

        var act = () => new Slots(Context).HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Slots_PlaceObjectFromStorage_InplaceSlot_ThrowsException()
    {
        var themePark = Faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark, worldFlatId: ThemeParkId);
        themePark.FillSlot(1, "test_inplace_attraction", 2);
        var player = Faker.Player(world: Faker.World(objects: [themePark]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var request = CreateRequest("placeObjectFromStorage", slotIndex: 1, objectData: CreateObjectData("test_inplace_attraction"));

        var act = () => new Slots(Context).HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    private static SlotsRequest CreateRequest(string operation, int? slotIndex = null, int? objectId = null, BuildingPlaceRequest? objectData = null)
    {
        return new SlotsRequest
        {
            ObjectId = ThemeParkId,
            Action = new SlotsActionRequest
            {
                Operation = operation,
                SlotIndex = slotIndex,
                ObjectId = objectId,
                ObjectData = objectData
            }
        };
    }

    private static BuildingPlaceRequest CreateObjectData(string itemName)
    {
        return new BuildingPlaceRequest
        {
            ItemName = itemName,
            ClassName = BuildingClassType.Attraction,
            State = WorldObjectState.Static,
            Direction = 1,
            Id = -10,
            Position = new PerformActionPositionRequest { X = 12, Y = 14, Z = 0 }
        };
    }
}
