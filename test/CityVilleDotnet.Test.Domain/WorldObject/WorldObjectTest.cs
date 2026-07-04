using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.WorldObject;

[Collection("Domain")]
public class WorldObjectTest(DomainFixture fixture)
{
    [Fact]
    public void WorldObject_MarkFreeItemGiven_SetsFlag()
    {
        var faker = new Faker();
        var building = faker.WorldObject();

        building.GivenFreeItem.Should().BeFalse();

        building.MarkFreeItemGiven();

        building.GivenFreeItem.Should().BeTrue();
    }
}
