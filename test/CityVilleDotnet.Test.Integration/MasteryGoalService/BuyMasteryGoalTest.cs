using AwesomeAssertions;
using CityVilleDotnet.Api.Services.MasteryGoalService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.MasteryGoalService;

[Collection("Database")]
public class BuyMasteryGoalTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task BuyMasteryGoal_NextLevelWithReward_ChargesProratedCostAndGivesReward()
    {
        var user = Faker.Player();
        user.SetCash(50);
        user.IncrementMastery("test_mastery_reward_crop", 3);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyMasteryGoal(Context);
        var request = new BuyMasteryGoalRequest { MasteryName = "test_mastery_reward_crop", MasteryLevel = 1 };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.Masteries)
            .Include(x => x.InventoryItems)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.Cash.Should().Be(30);
        player.Masteries.Should().ContainSingle(x => x.ItemName == "test_mastery_reward_crop" && x.Level == 2 && x.Count == 5);
        player.CountInventoryItem("test_mastery_reward_1").Should().Be(1);
        player.CountInventoryItem("test_mastery_reward_2").Should().Be(1);
    }
}
