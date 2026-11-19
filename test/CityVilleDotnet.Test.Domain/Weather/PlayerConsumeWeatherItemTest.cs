using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;

namespace CityVilleDotnet.Test.Domain.Weather;

// See NPC_rainman_1 in TestData/gameSettings.xml and qm_test_weather in TestData/questSettings.xml
[Collection("Domain")]
public class PlayerConsumeWeatherItemTest
{
    private const string Umbrella = "material_weather_umbrellas";

    [Fact]
    public void Player_ConsumeWeatherItem_RemovesOneUmbrella()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem(Umbrella, 2);

        player.ConsumeWeatherItem();

        player.InventoryItems.Should().ContainSingle(x => x.Name == Umbrella).Which.Amount.Should().Be(1);
    }

    [Fact]
    public void Player_ConsumeWeatherItem_LastUmbrella_RemovesInventoryItem()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem(Umbrella);

        player.ConsumeWeatherItem();

        player.InventoryItems.Should().NotContain(x => x.Name == Umbrella);
    }

    [Fact]
    public void Player_ConsumeWeatherItem_NoUmbrella_ThrowsException()
    {
        var faker = new Faker();
        var player = faker.Player();

        var act = () => player.ConsumeWeatherItem();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Player_ConsumeWeatherItem_GivesRainHelperTableRewards()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem(Umbrella);
        var gold = player.Gold;

        var secureRands = player.ConsumeWeatherItem();

        player.Gold.Should().Be(gold + 25);
        secureRands.Should().ContainSingle();
    }

    [Fact]
    public void Player_ConsumeWeatherItem_ProgressesShowersTaskOnly()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddItem(Umbrella);
        var quest = faker.Quest(name: "qm_test_weather", length: 2);
        player.Quests.Add(quest);

        player.ConsumeWeatherItem();

        quest.Progress.Should().Equal(1, 0);
    }
}
