using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WeatherService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WeatherService;

// See NPC_rainman_1 in TestData/gameSettings.xml and qm_test_weather in TestData/questSettings.xml
[Collection("Database")]
public class ConsumeWeatherItemTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const string Umbrella = "material_weather_umbrellas";

    [Fact]
    public async Task ConsumeWeatherItem_WithUmbrella_ConsumesAndRewards()
    {
        var user = Faker.Player();
        user.AddItem(Umbrella, 2);
        user.Quests.Add(Faker.Quest(name: "qm_test_weather", length: 2));
        var gold = user.Gold;

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new ConsumeWeatherItem(Context);

        var response = await handler.HandlePacket([], user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        Context.ChangeTracker.Clear();

        var player = await Context.Set<Player>()
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.InventoryItems.Should().ContainSingle(x => x.Name == Umbrella).Which.Amount.Should().Be(1);
        player.Gold.Should().Be(gold + 25);
        player.Quests.Should().ContainSingle(x => x.Name == "qm_test_weather").Which.Progress.Should().Equal(1, 0);
    }

    [Fact]
    public async Task ConsumeWeatherItem_NoUmbrella_ThrowsException()
    {
        var user = Faker.Player();

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new ConsumeWeatherItem(Context);

        var act = () => handler.HandlePacket([], user.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }
}
