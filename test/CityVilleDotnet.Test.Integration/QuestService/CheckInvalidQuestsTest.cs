using AwesomeAssertions;
using CityVilleDotnet.Api.Services.QuestService;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.QuestService;

[Collection("Database")]
public class CheckInvalidQuestsTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task CheckInvalidQuests_ActiveQuest_MarksExpired()
    {
        var user = Faker.Player();
        var quest = Faker.Quest();
        user.Quests.Add(quest);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new CheckInvalidQuests(Context, NullLogger<CheckInvalidQuests>.Instance);
        var request = new CheckInvalidQuestsRequest { QuestNames = [quest.Name] };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var savedQuest = await Context.Set<Domain.Entities.Quest>().FirstAsync(x => x.Name == quest.Name, TestContext.Current.CancellationToken);
        savedQuest.QuestType.Should().Be(QuestType.Expired);
    }

    [Fact]
    public async Task CheckInvalidQuests_CompletedQuest_IsNotModified()
    {
        var user = Faker.Player();
        var quest = Faker.Quest(questType: QuestType.Completed);
        user.Quests.Add(quest);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new CheckInvalidQuests(Context, NullLogger<CheckInvalidQuests>.Instance);
        var request = new CheckInvalidQuestsRequest { QuestNames = [quest.Name] };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var savedQuest = await Context.Set<Domain.Entities.Quest>().FirstAsync(x => x.Name == quest.Name, TestContext.Current.CancellationToken);
        savedQuest.QuestType.Should().Be(QuestType.Completed);
    }

    [Fact]
    public async Task CheckInvalidQuests_MultipleQuests_OnlyGivenQuestsExpire()
    {
        var user = Faker.Player();
        var invalidQuest = Faker.Quest();
        var validQuest = Faker.Quest();
        user.Quests.Add(invalidQuest);
        user.Quests.Add(validQuest);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new CheckInvalidQuests(Context, NullLogger<CheckInvalidQuests>.Instance);
        var request = new CheckInvalidQuestsRequest { QuestNames = [invalidQuest.Name] };

        await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        var savedInvalidQuest = await Context.Set<Domain.Entities.Quest>().FirstAsync(x => x.Name == invalidQuest.Name, TestContext.Current.CancellationToken);
        var savedValidQuest = await Context.Set<Domain.Entities.Quest>().FirstAsync(x => x.Name == validQuest.Name, TestContext.Current.CancellationToken);

        savedInvalidQuest.QuestType.Should().Be(QuestType.Expired);
        savedValidQuest.QuestType.Should().Be(QuestType.Active);
    }

    [Fact]
    public async Task CheckInvalidQuests_EmptyList_ReturnsEmptyResponse()
    {
        var handler = new CheckInvalidQuests(Context, NullLogger<CheckInvalidQuests>.Instance);
        var request = new CheckInvalidQuestsRequest { QuestNames = [] };

        var response = await handler.HandlePacket(request, Guid.NewGuid(), TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
    }
}
