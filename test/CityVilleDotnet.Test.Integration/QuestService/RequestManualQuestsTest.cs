using AwesomeAssertions;
using CityVilleDotnet.Api.Services.QuestService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.User;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.QuestService;

[Collection("Database")]
public class RequestManualQuestsTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task RequestManualQuests_ValidQuest()
    {
        var user = Faker.User();

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new RequestManualQuests(Context, NullLogger<RequestManualQuests>.Instance);
        var request = new RequestManualQuestsRequest
        {
            Quests = ["qm_test_quest_with_sequel"]
        };

        var response = await handler.HandlePacket(request, user.UserId, CancellationToken.None);

        var data = response["data"] as List<ASObject>;
        
        data.Should().NotBeNull();
        data.Should().HaveCount(1);
        data[0]["errorType"].Should().Be(0);
        data[0]["questName"].Should().Be("qm_test_quest_with_sequel");
        data[0]["questStarted"].Should().Be(true);

        var quest = await Context.Set<Quest>().FirstOrDefaultAsync(x => x.Name == "qm_test_quest_with_sequel");
        quest.Should().NotBeNull();
        quest.QuestType.Should().Be(Domain.Enums.QuestType.Active);
    }

    [Fact]
    public async Task RequestManualQuests_LevelBelowRequired_DoesNotStartQuest()
    {
        var user = Faker.User();

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new RequestManualQuests(Context, NullLogger<RequestManualQuests>.Instance);
        var request = new RequestManualQuestsRequest
        {
            Quests = ["qm_test_quest_high_level"]
        };

        var response = await handler.HandlePacket(request, user.UserId, CancellationToken.None);

        var data = response["data"] as List<ASObject>;
        data.Should().NotBeNull();
        data.Should().HaveCount(0);

        var quest = await Context.Set<Quest>().FirstOrDefaultAsync(x => x.Name == "qm_test_quest_high_level");
        quest.Should().BeNull();
    }

    [Fact]
    public async Task RequestManualQuests_LevelAboveRequired_StartsQuest()
    {
        var user = Faker.User();
        user.Player!.SetLevel(50);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new RequestManualQuests(Context, NullLogger<RequestManualQuests>.Instance);
        var request = new RequestManualQuestsRequest
        {
            Quests = ["qm_test_quest_high_level"]
        };

        var response = await handler.HandlePacket(request, user.UserId, CancellationToken.None);

        var data = response["data"] as List<ASObject>;
        data.Should().NotBeNull();
        data.Should().HaveCount(1);
        data![0]["questStarted"].Should().Be(true);

        var quest = await Context.Set<Quest>().FirstOrDefaultAsync(x => x.Name == "qm_test_quest_high_level");
        quest.Should().NotBeNull();
        quest!.QuestType.Should().Be(Domain.Enums.QuestType.Active);
    }
}
