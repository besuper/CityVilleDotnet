using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.WorldObject;

[Collection("Domain")]
public class WorldObjectThemeParkDtoTest
{
    [Fact]
    public void WorldObject_ToDto_ThemePark_SendsSlotStates()
    {
        var faker = new Faker();
        var themePark = faker.WorldObject(itemName: "test_theme_park", className: BuildingClassType.ThemePark);
        themePark.FillSlot(1, "test_inplace_attraction", 2);

        var dto = themePark.ToDto();

        dto.MechanicData["slots"].Should().BeEquivalentTo(new List<object> { "empty", "filled" });
    }

    [Fact]
    public void WorldObject_ToDto_DerivedThemePark_InheritsSlotStorage()
    {
        var faker = new Faker();
        var themePark = faker.WorldObject(itemName: "test_theme_park_level2", className: BuildingClassType.ThemePark);

        var dto = themePark.ToDto();

        dto.MechanicData["slots"].Should().BeEquivalentTo(new List<object> { "empty", "empty" });
    }
}
