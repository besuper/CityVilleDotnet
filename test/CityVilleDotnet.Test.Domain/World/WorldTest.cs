using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
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

    [Fact]
    public void World_AddBonusPopulation_SpreadsOverResidences()
    {
        var faker = new Faker();
        // res_cottage3 population is min=20 max=80, so each residence can hold 60 bonus
        var firstResidence = faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence);
        var secondResidence = faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence);
        var world = faker.World(objects: [firstResidence, secondResidence]);

        var applied = world.AddBonusPopulation(70);

        applied.Should().Be(70);
        firstResidence.GetBonusPopulation().Should().Be(60);
        secondResidence.GetBonusPopulation().Should().Be(10);
        world.GetCurrentPopulation().Should().Be(70);
    }

    [Fact]
    public void World_AddBonusPopulation_NoCapacityLeft_ReturnsOnlyApplied()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence);
        residence.AddBonusPopulation(60);
        var world = faker.World(objects: [residence]);

        var applied = world.AddBonusPopulation(10);

        applied.Should().Be(0);
        world.GetCurrentPopulation().Should().Be(0);
    }

    [Fact]
    public void World_CalculatePopulation_IncludesBonusPopulation()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence);
        residence.AddBonusPopulation(15);
        var world = faker.World(objects: [residence]);

        world.CalculatePopulation();

        world.GetCurrentPopulation().Should().Be(35);
        world.PopulationMin.Should().Be(20);
        world.PopulationMax.Should().Be(80);
    }
}
