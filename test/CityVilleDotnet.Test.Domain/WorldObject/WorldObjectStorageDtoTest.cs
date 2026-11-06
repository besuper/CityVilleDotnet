using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.WorldObject;

// See test_storage_* in TestData/gameSettings.xml
[Collection("Domain")]
public class WorldObjectStorageDtoTest
{
    [Fact]
    public void World_ToDto_StoredObjects_AreSentInsideTheirContainerInSlotOrder()
    {
        var faker = new Faker();
        var hood = faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, worldFlatId: 1);
        var first = faker.WorldObject(itemName: "test_family_house", worldFlatId: 11);
        var second = faker.WorldObject(itemName: "test_family_house", worldFlatId: 10);
        var world = faker.World(objects: [hood, second, first]);
        var hoodItem = GameSettingsManager.Instance.GetItem("test_storage_hood")!;
        world.StoreObject(hood, hoodItem, first);
        world.StoreObject(hood, hoodItem, second);

        var dto = world.ToDto();

        dto.Objects.Should().ContainSingle().Which.WorldFlatId.Should().Be(1);
        var slots = dto.Objects[0].MechanicData["slots"].Should().BeAssignableTo<List<object>>().Subject;
        slots.Cast<WorldObjectDto>().Select(x => x.WorldFlatId).Should().Equal(11, 10);
    }

    [Fact]
    public void World_ToDto_EmptyContainer_SendsEmptySlots()
    {
        var faker = new Faker();
        var mall = faker.WorldObject(itemName: "test_storage_mall", className: BuildingClassType.Mall);
        var world = faker.World(objects: [mall]);

        var dto = world.ToDto();

        dto.Objects[0].MechanicData["slots"].Should().BeAssignableTo<List<object>>().Which.Should().BeEmpty();
    }

    [Fact]
    public void World_ToDto_SuperNeighborhood_SendsNestedStoredObjects()
    {
        var faker = new Faker();
        var superHood = faker.WorldObject(itemName: "test_storage_superhood", className: BuildingClassType.SuperNeighborhood, worldFlatId: 1);
        var hood = faker.WorldObject(itemName: "test_storage_hood", className: BuildingClassType.Neighborhood, worldFlatId: 2);
        var house = faker.WorldObject(itemName: "test_family_house", worldFlatId: 3);
        var world = faker.World(objects: [superHood, hood, house]);
        world.StoreObject(hood, GameSettingsManager.Instance.GetItem("test_storage_hood")!, house);
        world.StoreObject(superHood, GameSettingsManager.Instance.GetItem("test_storage_superhood")!, hood);

        var dto = world.ToDto();

        var storedHood = dto.Objects.Should().ContainSingle().Subject.MechanicData["slots"].Should().BeAssignableTo<List<object>>().Which.Should().ContainSingle().Subject;
        var storedHoodDto = storedHood.Should().BeOfType<WorldObjectDto>().Subject;
        storedHoodDto.WorldFlatId.Should().Be(2);
        storedHoodDto.MechanicData["slots"].Should().BeAssignableTo<List<object>>().Which.Cast<WorldObjectDto>().Select(x => x.WorldFlatId).Should().Equal(3);
    }

    [Fact]
    public void WorldObject_ToDto_WithoutObjectStorage_HasNoSlots()
    {
        var faker = new Faker();
        var house = faker.WorldObject(itemName: "test_family_house");

        var dto = house.ToDto();

        dto.MechanicData.Should().NotContainKey("slots");
    }
}
