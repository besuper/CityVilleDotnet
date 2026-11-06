using System.Collections;
using AwesomeAssertions;
using CityVilleDotnet.Api.Common.GameWorlds;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

// See test_storage_* in TestData/gameSettings.xml
[Collection("Database")]
public class SlotsObjectStorageTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int HoodId = 1;
    private const int FirstHouseId = 10;
    private const int SecondHouseId = 11;
    private const int ThirdHouseId = 12;

    private async Task<Player> SeedHood(Action<WorldObject>? configureHood = null, params string[] houseNames)
    {
        var hood = Faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, x: 1, y: 1, worldFlatId: HoodId);
        configureHood?.Invoke(hood);
        var houses = houseNames.Select((name, i) => Faker.WorldObject(itemName: name, x: 10 + i, y: 10, worldFlatId: FirstHouseId + i));
        var player = Faker.Player(world: Faker.World(objects: [hood, .. houses]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        return player;
    }

    private Task<ASObject> Store(Player player, int objectId)
    {
        var request = new SlotsRequest { ObjectId = HoodId, Action = new SlotsActionRequest { Operation = "storeObject", ObjectId = objectId } };

        return new Slots(Context).HandlePacket(request, player.Id, TestContext.Current.CancellationToken);
    }

    private Task<ASObject> Remove(Player player, int slotId)
    {
        var request = new SlotsRequest { ObjectId = HoodId, Action = new SlotsActionRequest { Operation = "removeObject", SlotId = slotId, NewX = 20, NewY = 22, Dir = 2 } };

        return new Slots(Context).HandlePacket(request, player.Id, TestContext.Current.CancellationToken);
    }

    private async Task<List<WorldObject>> LoadObjects()
    {
        Context.ChangeTracker.Clear();

        return await Context.Set<WorldObject>().AsNoTracking().Where(x => x.ItemName.StartsWith("test_")).ToListAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task Slots_StoreObject_ValidResidence_KeepsObjectInWorldLinkedToContainer()
    {
        var player = await SeedHood(null, "test_family_house");

        var response = await Store(player, FirstHouseId);

        response["errorType"].Should().Be(0);
        var house = (await LoadObjects()).Single(x => x.WorldFlatId == FirstHouseId);
        house.StoredInId.Should().Be(HoodId);
        house.StorageSlotIndex.Should().Be(0);
    }

    [Fact]
    public async Task Slots_StoreObject_SecondCall_LoadsAlreadyStoredObjectsAndUsesNextSlot()
    {
        var player = await SeedHood(null, "test_family_house", "test_family_house");
        await Store(player, FirstHouseId);
        Context.ChangeTracker.Clear();

        await Store(player, SecondHouseId);

        var objects = await LoadObjects();
        objects.Single(x => x.WorldFlatId == SecondHouseId).StorageSlotIndex.Should().Be(1);
    }

    [Fact]
    public async Task Slots_StoreObject_CapacityReached_ThrowsException()
    {
        var player = await SeedHood(null, "test_family_house", "test_family_house", "test_family_house");
        await Store(player, FirstHouseId);
        Context.ChangeTracker.Clear();
        await Store(player, SecondHouseId);
        Context.ChangeTracker.Clear();

        var act = () => Store(player, ThirdHouseId);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Slots_StoreObject_CrewMember_LoadedToIncreaseCapacity()
    {
        var player = await SeedHood(hood => hood.AddCrewMember(null), "test_family_house", "test_family_house", "test_family_house");
        await Store(player, FirstHouseId);
        Context.ChangeTracker.Clear();
        await Store(player, SecondHouseId);
        Context.ChangeTracker.Clear();

        var response = await Store(player, ThirdHouseId);

        response["errorType"].Should().Be(0);
        (await LoadObjects()).Count(x => x.StoredInId == HoodId).Should().Be(3);
    }

    [Fact]
    public async Task Slots_StoreObject_RejectedObject_ThrowsAndKeepsItOnTheMap()
    {
        var player = await SeedHood(null, "test_plain_house");

        var act = () => Store(player, FirstHouseId);

        await act.Should().ThrowAsync<Exception>();
        (await LoadObjects()).Single(x => x.WorldFlatId == FirstHouseId).StoredInId.Should().BeNull();
    }

    [Fact]
    public async Task Slots_StoreObject_ProgressesStoreQuestTasks()
    {
        var hood = Faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, worldFlatId: HoodId);
        var house = Faker.WorldObject(itemName: "test_family_house", worldFlatId: FirstHouseId);
        var player = Faker.Player(world: Faker.World(objects: [hood, house]));
        player.Quests.Add(Faker.Quest(name: "qm_test_store_in_mechanic", length: 2));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        await Store(player, FirstHouseId);

        Context.ChangeTracker.Clear();
        var quest = await Context.Set<Quest>().AsNoTracking().SingleAsync(x => x.Name == "qm_test_store_in_mechanic", TestContext.Current.CancellationToken);
        quest.Progress.Should().Equal(1, 1);
    }

    [Fact]
    public async Task Slots_RemoveObject_PlacesObjectAndShiftsNextSlot()
    {
        var player = await SeedHood(null, "test_family_house", "test_family_house");
        await Store(player, FirstHouseId);
        Context.ChangeTracker.Clear();
        await Store(player, SecondHouseId);
        Context.ChangeTracker.Clear();

        var response = await Remove(player, 0);

        response["errorType"].Should().Be(0);
        var objects = await LoadObjects();
        var placed = objects.Single(x => x.WorldFlatId == FirstHouseId);
        placed.StoredInId.Should().BeNull();
        placed.StorageSlotIndex.Should().BeNull();
        placed.X.Should().Be(20);
        placed.Y.Should().Be(22);
        placed.Direction.Should().Be(2);
        objects.Single(x => x.WorldFlatId == SecondHouseId).StorageSlotIndex.Should().Be(0);
    }

    [Fact]
    public async Task Slots_RemoveObject_EmptySlot_ThrowsException()
    {
        var player = await SeedHood(null, "test_family_house");

        var act = () => Remove(player, 0);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Slots_StoreObject_WorldUpdate_RemovesStoredObjectAndSendsContainerSlots()
    {
        var player = await SeedHood(null, "test_family_house");
        using var tracker = new WorldUpdateTracker(Context);

        var response = await Store(player, FirstHouseId);
        await tracker.AppendToAsync(response, TestContext.Current.CancellationToken);

        var objects = (ASObject)((ASObject)((ASObject)response["metadata"])["worldUpdate"])["objects"];
        objects[FirstHouseId.ToString()].Should().BeNull();
        var container = (ASObject)objects[HoodId.ToString()];
        var slots = (IEnumerable)((ASObject)container["mechanicData"])["slots"];
        slots.Cast<object>().Should().ContainSingle();
    }

    [Fact]
    public async Task Slots_StoreObject_NeighborhoodInSuperNeighborhood_WorldUpdateKeepsNestedResidences()
    {
        const int superHoodId = 2;
        var hood = Faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, worldFlatId: HoodId);
        var superHood = Faker.WorldObject(itemName: "test_storage_superhood", className: BuildingClassType.SuperNeighborhood, worldFlatId: superHoodId);
        var house = Faker.WorldObject(itemName: "test_family_house", worldFlatId: FirstHouseId);
        var world = Faker.World(objects: [hood, superHood, house]);
        world.StoreObject(hood, GameSettingsManager.Instance.GetItem("test_storage_hood")!, house);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        using var tracker = new WorldUpdateTracker(Context);
        var request = new SlotsRequest { ObjectId = superHoodId, Action = new SlotsActionRequest { Operation = "storeObject", ObjectId = HoodId } };

        var response = await new Slots(Context).HandlePacket(request, player.Id, TestContext.Current.CancellationToken);
        await tracker.AppendToAsync(response, TestContext.Current.CancellationToken);

        var objects = (ASObject)((ASObject)((ASObject)response["metadata"])["worldUpdate"])["objects"];
        objects[HoodId.ToString()].Should().BeNull();
        objects.ContainsKey(FirstHouseId.ToString()).Should().BeFalse();
        var storedHood = ((IEnumerable)((ASObject)((ASObject)objects[superHoodId.ToString()])["mechanicData"])["slots"]).Cast<ASObject>().Single();
        var residences = (IEnumerable)((ASObject)storedHood["mechanicData"])["slots"];
        residences.Cast<object>().Should().ContainSingle();
    }
}
