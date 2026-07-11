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

    [Fact]
    public void WorldObject_StartRemodel_SetsSkinAndResetsBuilds()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "test_res_base");

        residence.StartRemodel("test_res_skin");

        residence.IsRemodeling().Should().BeTrue();
        residence.RemodelItemName.Should().Be("test_res_skin");
        residence.RemodelBuilds.Should().Be(0);
    }

    [Fact]
    public void WorldObject_AddRemodelBuild_ReturnsTrueWhenGateIsReached()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "test_res_base");
        residence.StartRemodel("test_res_skin");

        residence.AddRemodelBuild().Should().BeFalse();
        residence.AddRemodelBuild().Should().BeFalse();
        residence.AddRemodelBuild().Should().BeTrue();

        residence.RemodelBuilds.Should().Be(3);
    }

    [Fact]
    public void WorldObject_AddRemodelBuild_NotRemodeling_ThrowsException()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "test_res_base");

        var act = () => residence.AddRemodelBuild();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void WorldObject_GetRemodelRequiredBuilds_SkinItem_ResolvesFromBaseItem()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "test_res_skin");
        residence.StartRemodel("test_res_skin_premium");

        residence.GetRemodelRequiredBuilds().Should().Be(3);
    }

    [Fact]
    public void WorldObject_FinishRemodel_SwapsItemNameAndReturnsXp()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "test_res_base");
        residence.StartRemodel("test_res_skin");

        var xp = residence.FinishRemodel();

        xp.Should().Be(4);
        residence.ItemName.Should().Be("test_res_skin");
        residence.IsRemodeling().Should().BeFalse();
        residence.RemodelBuilds.Should().BeNull();
    }

    [Fact]
    public void WorldObject_FinishRemodel_NotRemodeling_ThrowsException()
    {
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "test_res_base");

        var act = () => residence.FinishRemodel();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void WorldObject_RollRandomZooAnimal_ReturnsAnimalFromLootTableAndStoresIt()
    {
        var faker = new Faker();
        var enclosure = faker.WorldObject(itemName: "test_enclosure");

        var animal = enclosure.RollRandomZooAnimal();

        animal.Should().BeOneOf("test_animal_common", "test_animal_uncommon", "test_animal_rare");
        enclosure.StorageItems.Should().ContainSingle(x => x.Name == animal);
    }

    [Fact]
    public void WorldObject_RollRandomZooAnimal_NoLootTable_ThrowsException()
    {
        var faker = new Faker();
        var enclosure = faker.WorldObject(itemName: "unknown_enclosure");

        var act = () => enclosure.RollRandomZooAnimal();

        act.Should().Throw<Exception>();
    }
}
