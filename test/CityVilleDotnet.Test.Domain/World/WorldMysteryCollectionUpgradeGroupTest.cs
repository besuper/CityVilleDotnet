using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.World;

// IMysteryCollectionManager::getUpgradeGroupName returns the highest headquarters level built, 1 by default
// Test settings only define mun_national_park_hq_1 to mun_national_park_hq_3
[Collection("Domain")]
public class WorldMysteryCollectionUpgradeGroupTest
{
    [Fact]
    public void World_GetMysteryCollectionUpgradeGroup_NoHeadquarters_ReturnsOne()
    {
        var faker = new Faker();
        var world = faker.World();

        world.GetMysteryCollectionUpgradeGroup("NationalParkManager").Should().Be(1);
    }

    [Fact]
    public void World_GetMysteryCollectionUpgradeGroup_ReturnsHeadquartersLevel()
    {
        var faker = new Faker();
        var headquarters = faker.WorldObject(itemName: "mun_national_park_hq_2", className: BuildingClassType.Municipal);
        var world = faker.World(objects: [headquarters]);

        world.GetMysteryCollectionUpgradeGroup("NationalParkManager").Should().Be(2);
    }

    [Fact]
    public void World_GetMysteryCollectionUpgradeGroup_SeveralHeadquarters_ReturnsHighestLevel()
    {
        var faker = new Faker();
        var firstHeadquarters = faker.WorldObject(itemName: "mun_national_park_hq_3", className: BuildingClassType.Municipal);
        var secondHeadquarters = faker.WorldObject(itemName: "mun_national_park_hq_1", className: BuildingClassType.Municipal);
        var world = faker.World(objects: [firstHeadquarters, secondHeadquarters]);

        world.GetMysteryCollectionUpgradeGroup("NationalParkManager").Should().Be(3);
    }

    [Fact]
    public void World_GetMysteryCollectionUpgradeGroup_HeadquartersNotInSettings_IsIgnored()
    {
        var faker = new Faker();
        var headquarters = faker.WorldObject(itemName: "mun_national_park_hq_4", className: BuildingClassType.Municipal);
        var world = faker.World(objects: [headquarters]);

        world.GetMysteryCollectionUpgradeGroup("NationalParkManager").Should().Be(1);
    }

    [Fact]
    public void World_GetMysteryCollectionUpgradeGroup_OtherManagerHeadquarters_IsIgnored()
    {
        var faker = new Faker();
        var headquarters = faker.WorldObject(itemName: "mun_national_park_hq_2", className: BuildingClassType.Municipal);
        var world = faker.World(objects: [headquarters]);

        world.GetMysteryCollectionUpgradeGroup("AlpsVillageManager").Should().Be(1);
    }

    [Fact]
    public void World_GetMysteryCollectionUpgradeGroup_UnknownManager_ThrowsDomainException()
    {
        var faker = new Faker();
        var world = faker.World();

        var act = () => world.GetMysteryCollectionUpgradeGroup("UnknownManager");

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidData);
    }
}
