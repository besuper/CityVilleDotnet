using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.MapRect;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.World;

[Collection("Domain")]
public class WorldTest
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
    public void World_GetBuildingByClientId_NotFound_ThrowsDomainException()
    {
        var faker = new Faker();
        var building = faker.WorldObject(worldFlatId: 42);
        var world = faker.World(objects: [building]);

        var act = () => world.GetBuildingByClientId(999);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.ForceReload);
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

    [Fact]
    public void World_HasRemodelHeadquarters_WithHeadquarters_ReturnsTrue()
    {
        var faker = new Faker();
        var headquarters = faker.WorldObject(itemName: "mun_constructioncompany", className: BuildingClassType.Municipal);
        var world = faker.World(objects: [headquarters]);

        world.HasRemodelHeadquarters().Should().BeTrue();
    }

    [Fact]
    public void World_HasRemodelHeadquarters_WithoutHeadquarters_ReturnsFalse()
    {
        var faker = new Faker();
        var world = faker.World(objects: [faker.WorldObject(itemName: "res_cottage3")]);

        world.HasRemodelHeadquarters().Should().BeFalse();
    }

    [Fact]
    public void World_CountConstructionOrBuildingByName_CountsFinishedAndConstructionTarget()
    {
        var faker = new Faker();
        var finished = faker.WorldObject(itemName: "mun_visitorcenter", className: BuildingClassType.Municipal);
        var underConstruction = faker.WorldObject(itemName: "mun_visitorcenter", className: BuildingClassType.Municipal);
        underConstruction.SetAsConstructionSite("construction_3x3_2stage", 2);
        var world = faker.World(objects: [finished, underConstruction]);

        world.CountConstructionOrBuildingByName("mun_visitorcenter").Should().Be(2);
        world.CountBuildingByName("mun_visitorcenter").Should().Be(1);
    }

    [Theory]
    [InlineData(48, -84, true)]
    [InlineData(59, -73, true)]
    [InlineData(60, -84, false)]
    [InlineData(48, -72, false)]
    [InlineData(47, -84, false)]
    public void World_IsPointInTerritory_MatchesMapRectBounds(int x, int y, bool expected)
    {
        var faker = new Faker();
        var world = faker.World(mapRects: [faker.MapRect(x: 48, y: -84, width: 12, height: 12)]);

        world.IsPointInTerritory(x, y).Should().Be(expected);
    }

    // mun_drawbridge builds from test_drawbridge_construction in TestData/gameSettings.xml
    [Fact]
    public void World_AddDrawbridgeSite_AddsFinishableConstructionSite()
    {
        var faker = new Faker();
        var world = faker.World(objects: [faker.WorldObject(worldFlatId: 5)]);

        world.AddDrawbridgeSite();

        var site = world.Objects.Should().ContainSingle(x => x.TargetBuildingName == "mun_drawbridge").Subject;
        site.ItemName.Should().Be("test_drawbridge_construction");
        site.ClassName.Should().Be(BuildingClassType.ConstructionSite);
        site.TargetBuildingClass.Should().Be(BuildingClassType.PeepCapacityResource);
        site.X.Should().Be(56);
        site.Y.Should().Be(-93);
        site.WorldFlatId.Should().Be(6);
        site.IsConstructionComplete().Should().BeTrue();
        world.NextBuildingId.Should().Be(7);
    }
}
