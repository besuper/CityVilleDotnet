using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.InventoryItem;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_consume_items in TestData/questSettings.xml
[Collection("Domain")]
public class PlayerConsumeQuestInitItemsTest
{
    private const string QuestName = "qm_test_consume_items";

    [Fact]
    public void Player_ConsumeQuestInitItems_ConsumesItemsAndProgressesTask()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        player.Quests.Add(quest);
        player.InventoryItems.Add(faker.InventoryItem(itemName: "test_pamphlet", amount: 3));

        var removed = player.ConsumeQuestInitItems();

        quest.Progress.Should().Equal(3, 0);
        player.CountInventoryItem("test_pamphlet").Should().Be(0);
        removed.Should().ContainSingle().Which.Name.Should().Be("test_pamphlet");
    }

    [Fact]
    public void Player_ConsumeQuestInitItems_SumsEveryItemOfTheTask()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        player.Quests.Add(quest);
        player.InventoryItems.Add(faker.InventoryItem(itemName: "test_pamphlet", amount: 2));
        player.InventoryItems.Add(faker.InventoryItem(itemName: "test_flyer", amount: 2));

        var removed = player.ConsumeQuestInitItems();

        quest.Progress.Should().Equal(4, 0);
        player.InventoryItems.Should().BeEmpty();
        removed.Should().HaveCount(2);
    }

    [Fact]
    public void Player_ConsumeQuestInitItems_AddsToExistingProgress()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        quest.Progress[0] = 2;
        player.Quests.Add(quest);
        player.InventoryItems.Add(faker.InventoryItem(itemName: "test_pamphlet", amount: 3));

        player.ConsumeQuestInitItems();

        quest.Progress[0].Should().Be(5);
    }

    [Fact]
    public void Player_ConsumeQuestInitItems_CompletedTask_KeepsItems()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        quest.Progress[0] = 5;
        player.Quests.Add(quest);
        player.InventoryItems.Add(faker.InventoryItem(itemName: "test_pamphlet", amount: 3));

        var removed = player.ConsumeQuestInitItems();

        quest.Progress[0].Should().Be(5);
        player.CountInventoryItem("test_pamphlet").Should().Be(3);
        removed.Should().BeEmpty();
    }

    [Fact]
    public void Player_ConsumeQuestInitItems_PurchasedTask_KeepsItems()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        quest.PurchaseProgression(0);
        player.Quests.Add(quest);
        player.InventoryItems.Add(faker.InventoryItem(itemName: "test_pamphlet", amount: 3));

        player.ConsumeQuestInitItems();

        quest.Progress[0].Should().Be(0);
        player.CountInventoryItem("test_pamphlet").Should().Be(3);
    }

    [Fact]
    public void Player_ConsumeQuestInitItems_InactiveQuest_KeepsItems()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2, questType: QuestType.Completed);
        player.Quests.Add(quest);
        player.InventoryItems.Add(faker.InventoryItem(itemName: "test_pamphlet", amount: 3));

        player.ConsumeQuestInitItems();

        quest.Progress[0].Should().Be(0);
        player.CountInventoryItem("test_pamphlet").Should().Be(3);
    }

    [Fact]
    public void Player_ConsumeQuestInitItems_StoredItems_AreIgnored()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        player.Quests.Add(quest);
        player.InventoryItems.Add(faker.InventoryItem(itemName: "test_pamphlet", amount: 3, storageType: faker.Random.String2(10)));

        var removed = player.ConsumeQuestInitItems();

        quest.Progress[0].Should().Be(0);
        player.InventoryItems.Should().ContainSingle().Which.Amount.Should().Be(3);
        removed.Should().BeEmpty();
    }

    [Fact]
    public void Player_ConsumeQuestInitItems_NoItems_DoesNothing()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        player.Quests.Add(quest);

        var removed = player.ConsumeQuestInitItems();

        quest.Progress.Should().Equal(0, 0);
        removed.Should().BeEmpty();
    }

    [Fact]
    public void Player_HandleQuestsProgress_VisitorHelp_ProgressesMatchingHelpType()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("visitorHelp", "businessSendTour");
        player.HandleQuestsProgress("visitorHelp", "residenceCollectRent");

        quest.Progress.Should().Equal(0, 1);
    }
}
