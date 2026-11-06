using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.FranchiseLocation;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.World;

// See test_storage_* in TestData/gameSettings.xml
[Collection("Domain")]
public class WorldStorageTest
{
    private const int ContainerId = 1;

    private static GameItem Item(string name)
    {
        return GameSettingsManager.Instance.GetItem(name)!;
    }

    private static (CityVilleDotnet.Domain.Entities.World World, CityVilleDotnet.Domain.Entities.WorldObject Container, List<CityVilleDotnet.Domain.Entities.WorldObject> Objects) Setup(
        Faker faker, string containerName, BuildingClassType containerClass, params string[] objectNames)
    {
        var container = faker.WorldObject(itemName: containerName, className: containerClass, worldFlatId: ContainerId);
        var objects = objectNames.Select((name, i) => faker.WorldObject(itemName: name, worldFlatId: 10 + i)).ToList();

        return (faker.World(objects: [container, .. objects]), container, objects);
    }

    [Fact]
    public void World_StoreObject_ValidResidence_StoresInFirstSlot()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, "test_family_house");

        world.StoreObject(container, Item("test_storage_hood"), objects[0]);

        objects[0].StoredInId.Should().Be(ContainerId);
        objects[0].StorageSlotIndex.Should().Be(0);
        world.Objects.Should().Contain(objects[0]);
    }

    [Fact]
    public void World_StoreObject_SecondObject_UsesNextSlot()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, "test_family_house", "test_family_house");

        world.StoreObject(container, Item("test_storage_hood"), objects[0]);
        world.StoreObject(container, Item("test_storage_hood"), objects[1]);

        world.GetStoredObjects(container).Should().Equal(objects[0], objects[1]);
        objects[1].StorageSlotIndex.Should().Be(1);
    }

    [Fact]
    public void World_StoreObject_InitialCapacityReached_ThrowsException()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, "test_family_house", "test_family_house", "test_family_house");
        world.StoreObject(container, Item("test_storage_hood"), objects[0]);
        world.StoreObject(container, Item("test_storage_hood"), objects[1]);

        var act = () => world.StoreObject(container, Item("test_storage_hood"), objects[2]);

        act.Should().Throw<Exception>();
        objects[2].StoredInId.Should().BeNull();
    }

    [Fact]
    public void World_StoreObject_CrewMember_IncreasesCapacity()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, "test_family_house", "test_family_house", "test_family_house");
        container.AddCrewMember(null);

        foreach (var obj in objects)
        {
            world.StoreObject(container, Item("test_storage_hood"), obj);
        }

        world.GetStoredObjects(container).Should().HaveCount(3);
    }

    [Fact]
    public void WorldObject_GetStorageCapacity_ManyCrewMembers_CappedAtMaxCapacity()
    {
        var faker = new Faker();
        var container = faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood);

        for (var i = 0; i < 5; i++)
        {
            container.AddCrewMember(null);
        }

        container.GetStorageCapacity(Item("test_storage_hood").StorageUnit!).Should().Be(3);
    }

    [Theory]
    [InlineData("test_shop")]
    [InlineData("test_plain_house")]
    [InlineData("test_unstorable_house")]
    [InlineData("test_banned_house")]
    public void World_StoreObject_Neighborhood_RejectedObject_ThrowsException(string itemName)
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, itemName);

        var act = () => world.StoreObject(container, Item("test_storage_hood"), objects[0]);

        act.Should().Throw<Exception>();
        objects[0].StoredInId.Should().BeNull();
    }

    [Fact]
    public void World_StoreObject_DerivedContainer_InheritsStorageAndValidation()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood_level2", BuildingClassType.Neighborhood, "test_family_house", "test_plain_house");

        world.StoreObject(container, Item("test_storage_hood_level2"), objects[0]);
        var act = () => world.StoreObject(container, Item("test_storage_hood_level2"), objects[1]);

        objects[0].StoredInId.Should().Be(ContainerId);
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void World_StoreObject_Mall_Business_IsStored()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_mall", BuildingClassType.Mall, "test_shop");

        world.StoreObject(container, Item("test_storage_mall"), objects[0]);

        objects[0].StoredInId.Should().Be(ContainerId);
    }

    [Theory]
    [InlineData("test_premium_shop")]
    [InlineData("test_banned_shop")]
    [InlineData("test_family_house")]
    public void World_StoreObject_Mall_RejectedObject_ThrowsException(string itemName)
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_mall", BuildingClassType.Mall, itemName);

        var act = () => world.StoreObject(container, Item("test_storage_mall"), objects[0]);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void World_StoreObject_Mall_FranchiseBusiness_ThrowsException()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_mall", BuildingClassType.Mall, "test_shop");
        objects[0].SetFranchiseLocation(faker.FranchiseLocation(), faker.Random.String2(32));

        var act = () => world.StoreObject(container, Item("test_storage_mall"), objects[0]);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void World_StoreObject_MunicipalCenter_Municipal_IsStored()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_center", BuildingClassType.MunicipalCenter, "test_town_hall");

        world.StoreObject(container, Item("test_storage_center"), objects[0]);

        objects[0].StoredInId.Should().Be(ContainerId);
    }

    [Fact]
    public void World_StoreObject_MunicipalCenter_Residence_ThrowsException()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_center", BuildingClassType.MunicipalCenter, "test_family_house");

        var act = () => world.StoreObject(container, Item("test_storage_center"), objects[0]);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void World_StoreObject_StorageFacility_WithoutGlobalValidator_IsStored()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_facility", BuildingClassType.StorageFacility, "test_farm_silo");

        world.StoreObject(container, Item("test_storage_facility"), objects[0]);

        objects[0].StoredInId.Should().Be(ContainerId);
    }

    [Fact]
    public void World_StoreObject_SuperNeighborhood_StoresNeighborhood()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_superhood", BuildingClassType.SuperNeighborhood, "test_storage_hood");

        world.StoreObject(container, Item("test_storage_superhood"), objects[0]);

        objects[0].StoredInId.Should().Be(ContainerId);
    }

    [Fact]
    public void World_StoreObject_UnknownValidationFunction_ThrowsException()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_unknown_func", BuildingClassType.Neighborhood, "test_family_house");

        var act = () => world.StoreObject(container, Item("test_storage_unknown_func"), objects[0]);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void World_StoreObject_ContainerWithoutObjectStorage_ThrowsException()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_theme_park", BuildingClassType.ThemePark, "test_attraction");

        var act = () => world.StoreObject(container, Item("test_theme_park"), objects[0]);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void World_StoreObject_ItSelf_ThrowsException()
    {
        var faker = new Faker();
        var (world, container, _) = Setup(faker, "test_storage_superhood", BuildingClassType.SuperNeighborhood);

        var act = () => world.StoreObject(container, Item("test_storage_superhood"), container);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void World_StoreObject_AlreadyStoredObject_ThrowsException()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, "test_family_house");
        world.StoreObject(container, Item("test_storage_hood"), objects[0]);

        var act = () => world.StoreObject(container, Item("test_storage_hood"), objects[0]);

        act.Should().Throw<Exception>();
        world.GetStoredObjects(container).Should().ContainSingle();
    }

    [Fact]
    public void World_PlaceFromStorage_FirstSlot_PlacesObjectAndShiftsNextSlots()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, "test_family_house", "test_family_house");
        world.StoreObject(container, Item("test_storage_hood"), objects[0]);
        world.StoreObject(container, Item("test_storage_hood"), objects[1]);

        world.PlaceFromStorage(container, 0, 20, 21, 3);

        objects[0].StoredInId.Should().BeNull();
        objects[0].StorageSlotIndex.Should().BeNull();
        objects[0].X.Should().Be(20);
        objects[0].Y.Should().Be(21);
        objects[0].Direction.Should().Be(3);
        objects[1].StorageSlotIndex.Should().Be(0);
        world.GetStoredObjects(container).Should().Equal(objects[1]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void World_PlaceFromStorage_InvalidSlot_ThrowsException(int slotIndex)
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, "test_family_house");
        world.StoreObject(container, Item("test_storage_hood"), objects[0]);

        var act = () => world.PlaceFromStorage(container, slotIndex, 20, 21, 0);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void World_RemoveBuilding_ContainerStoringObjects_ThrowsException()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, "test_family_house");
        world.StoreObject(container, Item("test_storage_hood"), objects[0]);

        var act = () => world.RemoveBuilding(container);

        act.Should().Throw<Exception>();
        world.Objects.Should().Contain(container);
    }

    [Fact]
    public void World_RemoveBuilding_EmptyContainer_RemovesIt()
    {
        var faker = new Faker();
        var (world, container, _) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood);

        world.RemoveBuilding(container);

        world.Objects.Should().NotContain(container);
    }

    [Fact]
    public void World_GetBuildingByCoord_IgnoresStoredObjects()
    {
        var faker = new Faker();
        var container = faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, x: 1, y: 1, worldFlatId: ContainerId);
        var house = faker.WorldObject(itemName: "test_family_house", x: 5, y: 5, worldFlatId: 10);
        var world = faker.World(objects: [container, house]);
        world.StoreObject(container, Item("test_storage_hood"), house);

        world.GetBuildingByCoord(5, 5, 0).Should().BeNull();
    }

    [Fact]
    public void World_QuestCounts_OnlyClientSlottedAwareCountsIncludeStoredObjects()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_hood", BuildingClassType.Neighborhood, "test_family_house", "test_family_house");
        objects[0].StoreIn(container.WorldFlatId, 0);

        world.CountBuildingByName("test_family_house").Should().Be(2);
        world.CountBuildingByRegex("^test_family_house$").Should().Be(1);
        world.CountBuildingByNames(["test_family_house"]).Should().Be(1);
        world.CountBuildingByClassName(nameof(BuildingClassType.Residence)).Should().Be(1);
    }

    [Fact]
    public void World_GetHubLevel_StoredHub_IsIgnored()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_center", BuildingClassType.MunicipalCenter, "test_prey_hub_2");
        var setting = GameSettingsManager.Instance.GetPreyGroups().Single(x => x.Id == "test_prey");

        world.GetHubLevel(setting).Should().Be(2);
        objects[0].StoreIn(container.WorldFlatId, 0);
        world.GetHubLevel(setting).Should().Be(0);
    }

    [Fact]
    public void World_HasRemodelHeadquarters_StoredHeadquarters_IsIgnored()
    {
        var faker = new Faker();
        var (world, container, objects) = Setup(faker, "test_storage_center", BuildingClassType.MunicipalCenter, "mun_constructioncompany");
        objects[0].StoreIn(container.WorldFlatId, 0);

        world.HasRemodelHeadquarters().Should().BeFalse();
    }

    [Fact]
    public void World_GetMaxStoredObjectsByClass_CountsSubclassesAndKeepsMaximum()
    {
        var faker = new Faker();
        var hood = faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, worldFlatId: 1);
        var superHood = faker.WorldObject(itemName: "test_storage_superhood", className: BuildingClassType.SuperNeighborhood, worldFlatId: 2);
        var house = faker.WorldObject(itemName: "test_family_house", worldFlatId: 10);
        var storedHoods = new[] { faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, worldFlatId: 20), faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, worldFlatId: 21) };
        var world = faker.World(objects: [hood, superHood, house, .. storedHoods]);
        world.StoreObject(hood, Item("test_storage_hood"), house);

        foreach (var storedHood in storedHoods)
        {
            world.StoreObject(superHood, Item("test_storage_superhood"), storedHood);
        }

        world.GetMaxStoredObjectsByClass("Neighborhood").Should().Be(2);
        world.GetMaxStoredObjectsByClass("Mall").Should().Be(0);
    }

    [Fact]
    public void World_GetMaxFilledSlots_CountsOnlyRequestedSlots()
    {
        var faker = new Faker();
        var themePark = faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark);
        themePark.FillSlot(1, "test_inplace_attraction", 2);
        var world = faker.World(objects: [themePark]);

        world.GetMaxFilledSlots(["test_theme_park"], [0, 1]).Should().Be(1);
        world.GetMaxFilledSlots(["test_theme_park"], [0]).Should().Be(0);
        world.GetMaxFilledSlots(["test_other_park"], [1]).Should().Be(0);
    }
}
