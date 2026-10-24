using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.InventoryItem;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

// See qm_test_consume_items in TestData/questSettings.xml
[Collection("Database")]
public class PingFeedQuestsTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const string QuestName = "qm_test_consume_items";

    [Fact]
    public async Task PingFeedQuests_ConsumeItemsAtInit_SavesProgressAndDeletesItems()
    {
        var user = Faker.Player();
        user.Quests.Add(Faker.Quest(name: QuestName, length: 2));
        user.InventoryItems.Add(Faker.InventoryItem(itemName: "test_pamphlet", amount: 3));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new PingFeedQuests(Context);

        var response = await handler.HandlePacket([], user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        Context.ChangeTracker.Clear();

        var quest = await Context.Set<Quest>().FirstAsync(x => x.Name == QuestName, TestContext.Current.CancellationToken);
        quest.QuestType.Should().Be(QuestType.Active);
        quest.Progress.Should().Equal(3, 0);

        var hasItem = await Context.Set<InventoryItem>().AnyAsync(x => x.Name == "test_pamphlet", TestContext.Current.CancellationToken);
        hasItem.Should().BeFalse();
    }

    [Fact]
    public async Task PingFeedQuests_ConsumeItemsAtInit_CompletesQuestWhenAllTasksDone()
    {
        var user = Faker.Player();
        var quest = Faker.Quest(name: QuestName, length: 2);
        quest.Progress[1] = 5;
        user.Quests.Add(quest);
        user.InventoryItems.Add(Faker.InventoryItem(itemName: "test_pamphlet", amount: 2));
        user.InventoryItems.Add(Faker.InventoryItem(itemName: "test_flyer", amount: 3));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new PingFeedQuests(Context);

        var response = await handler.HandlePacket([], user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        Context.ChangeTracker.Clear();

        var updatedQuest = await Context.Set<Quest>().FirstAsync(x => x.Name == QuestName, TestContext.Current.CancellationToken);
        updatedQuest.QuestType.Should().Be(QuestType.Completed);

        var player = await Context.Set<Player>()
            .Include(x => x.InventoryItems)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);
        player.InventoryItems.Should().NotContain(x => x.Name == "test_pamphlet" || x.Name == "test_flyer");
    }
}
