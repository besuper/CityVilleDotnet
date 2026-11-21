using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.WorldObject;

// test_crew_upgrade: gate "build" needs 4 crew members, gate "pre_upgrade" needs 10
[Collection("Domain")]
public class WorldObjectCrewSlotsTest
{
    [Fact]
    public void WorldObject_GetFreeCrewSlots_NoGateName_UsesFirstGate()
    {
        var faker = new Faker();
        var building = faker.WorldObject(itemName: "test_crew_upgrade");
        building.AddCrewMember(null);

        building.GetFreeCrewSlots(null).Should().Be(3);
    }

    [Fact]
    public void WorldObject_GetFreeCrewSlots_NamedGate_UsesThatGate()
    {
        var faker = new Faker();
        var building = faker.WorldObject(itemName: "test_crew_upgrade");

        building.GetFreeCrewSlots("pre_upgrade").Should().Be(10);
    }

    [Fact]
    public void WorldObject_GetFreeCrewSlots_ConstructionSite_UsesTargetBuildingGate()
    {
        var faker = new Faker();
        var building = faker.WorldObject(itemName: "test_crew_upgrade");
        building.SetAsConstructionSite("test_crew_construction", 2);

        building.GetFreeCrewSlots(null).Should().Be(4);
    }

    [Fact]
    public void WorldObject_GetFreeCrewSlots_CrewComplete_ReturnsZero()
    {
        var faker = new Faker();
        var building = faker.WorldObject(itemName: "test_crew_upgrade");

        for (var i = 0; i < 5; i++)
        {
            building.AddCrewMember(null);
        }

        building.GetFreeCrewSlots(null).Should().Be(0);
    }

    [Fact]
    public void WorldObject_GetFreeCrewSlots_UnknownGateOrItem_ReturnsZero()
    {
        var faker = new Faker();

        faker.WorldObject(itemName: "test_crew_upgrade").GetFreeCrewSlots("test_unknown_gate").Should().Be(0);
        faker.WorldObject(itemName: faker.Random.String2(64)).GetFreeCrewSlots(null).Should().Be(0);
    }
}
