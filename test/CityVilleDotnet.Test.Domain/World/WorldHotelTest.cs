using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.World;

// See test_resort_* in TestData/gameSettings.xml (hotel 4x4 with radius 6.5, truncated to 6 like the client)
[Collection("Domain")]
public class WorldHotelTest
{
    private static CityVilleDotnet.Domain.Entities.WorldObject Hotel(Faker faker, int x, int y, WorldObjectState state = WorldObjectState.Open)
    {
        return faker.WorldObject(itemName: "test_resort_hotel", className: BuildingClassType.Hotel, state: state, x: x, y: y);
    }

    private static CityVilleDotnet.Domain.Entities.WorldObject Shop(Faker faker, int x, int y, string itemName = "test_resort_shop", BuildingClassType className = BuildingClassType.Business)
    {
        return faker.WorldObject(itemName: itemName, className: className, x: x, y: y);
    }

    [Fact]
    public void World_HasOpenHotel_OpenHotel_ReturnsTrue()
    {
        var faker = new Faker();
        var world = faker.World(objects: [Hotel(faker, 0, 0)]);

        world.HasOpenHotel().Should().BeTrue();
    }

    [Fact]
    public void World_HasOpenHotel_HarvestableHotel_ReturnsTrue()
    {
        var faker = new Faker();
        var world = faker.World(objects: [Hotel(faker, 0, 0, WorldObjectState.ClosedHarvestable)]);

        world.HasOpenHotel().Should().BeTrue();
    }

    [Fact]
    public void World_HasOpenHotel_ClosedHotel_ReturnsFalse()
    {
        var faker = new Faker();
        var world = faker.World(objects: [Hotel(faker, 0, 0, WorldObjectState.Closed)]);

        world.HasOpenHotel().Should().BeFalse();
    }

    [Fact]
    public void World_HasOpenHotel_OpenBusinessOnly_ReturnsFalse()
    {
        var faker = new Faker();
        var world = faker.World(objects: [faker.WorldObject(itemName: "test_resort_shop", className: BuildingClassType.Business, state: WorldObjectState.Open)]);

        world.HasOpenHotel().Should().BeFalse();
    }

    [Fact]
    public void World_GetMaxResortBusinessCount_NoHotel_ReturnsZero()
    {
        var faker = new Faker();
        var world = faker.World(objects: [Shop(faker, 4, 1)]);

        world.GetMaxResortBusinessCount().Should().Be(0);
    }

    [Fact]
    public void World_GetMaxResortBusinessCount_BusinessInRangeOfTwoHotels_CountsOnlyForClosest()
    {
        var faker = new Faker();
        var world = faker.World(objects:
        [
            Hotel(faker, 0, 0),
            Hotel(faker, 8, 0),
            Shop(faker, 4, 1),
            Shop(faker, 2, 4),
            Shop(faker, 6, 1)
        ]);

        world.GetMaxResortBusinessCount().Should().Be(2);
    }

    [Fact]
    public void World_GetMaxResortBusinessCount_RadiusIsTruncated_IgnoresBusinessBeyondIntegerRadius()
    {
        var faker = new Faker();
        var world = faker.World(objects:
        [
            Hotel(faker, 0, 0),
            Shop(faker, 6, 4, "test_resort_kiosk"),
            Shop(faker, 7, 4, "test_resort_kiosk")
        ]);

        world.GetMaxResortBusinessCount().Should().Be(1);
    }

    [Fact]
    public void World_GetMaxResortBusinessCount_CountsSocialBusinessesAndIgnoresOtherClasses()
    {
        var faker = new Faker();
        var world = faker.World(objects:
        [
            Hotel(faker, 0, 0),
            Shop(faker, 4, 1, className: BuildingClassType.SocialBusiness),
            Shop(faker, 2, 4, className: BuildingClassType.Residence)
        ]);

        world.GetMaxResortBusinessCount().Should().Be(1);
    }
}
