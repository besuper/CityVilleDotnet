using System.Text.Json;
using AwesomeAssertions;

namespace CityVilleDotnet.Test.Domain.WorldDto;

public class WorldDtoTest
{
    [Theory]
    [InlineData("Resources/startWorld.json")]
    [InlineData("Resources/startWorldDowntown.json")]
    public void WorldDto_Deserialize_StartWorldJson_DoesNotThrow(string path)
    {
        var json = File.ReadAllText(path);

        var world = JsonSerializer.Deserialize<CityVilleDotnet.Domain.GameEntities.WorldDto>(json);

        world.Should().NotBeNull();
        world.Objects.Should().NotBeEmpty();
    }
}
