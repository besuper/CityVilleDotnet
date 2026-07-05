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

    [Fact]
    public void WorldObject_AddBonusPopulation_ClampsToMaxIncrease()
    {
        var faker = new Faker();
        // res_cottage3 population is min=20 max=80, so the bonus is capped at 60
        var residence = faker.WorldObject(itemName: "res_cottage3");

        var applied = residence.AddBonusPopulation(100);

        applied.Should().Be(60);
        residence.GetBonusPopulation().Should().Be(60);
    }

    [Fact]
    public void WorldObject_AddBonusPopulation_AccumulatesAcrossCalls()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "res_cottage3");

        residence.AddBonusPopulation(10).Should().Be(10);
        residence.AddBonusPopulation(10).Should().Be(10);

        residence.GetBonusPopulation().Should().Be(20);
    }

    [Fact]
    public void WorldObject_AddBonusPopulation_ItemWithoutPopulation_ReturnsZero()
    {
        var faker = new Faker();
        var decoration = faker.WorldObject(itemName: "deco_tree");

        var applied = decoration.AddBonusPopulation(10);

        applied.Should().Be(0);
        decoration.GetBonusPopulation().Should().Be(0);
    }
}
