using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.World;

[Collection("Domain")]
public class WorldTest(DomainFixture fixture)
{
    [Fact]
    public void World_GetBuildingByClientId_MatchesWorldFlatId()
    {
        var faker = new Faker();
        var building = faker.WorldObject(worldFlatId: 42);
        var world = faker.World(objects: [building]);

        var result = world.GetBuildingByClientId(42);

        result.Should().Be(building);
    }

    [Fact]
    public void World_GetBuildingByClientId_MatchesTempId()
    {
        var faker = new Faker();
        var building = faker.WorldObject(tempId: 16777220, worldFlatId: 42);
        var world = faker.World(objects: [building]);

        var result = world.GetBuildingByClientId(16777220);

        result.Should().Be(building);
    }

    [Fact]
    public void World_GetBuildingByClientId_NotFound_ReturnsNull()
    {
        var faker = new Faker();
        var building = faker.WorldObject(worldFlatId: 42);
        var world = faker.World(objects: [building]);

        var result = world.GetBuildingByClientId(999);

        result.Should().BeNull();
    }
}
