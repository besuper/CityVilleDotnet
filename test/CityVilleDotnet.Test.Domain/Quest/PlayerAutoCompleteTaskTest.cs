using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;

namespace CityVilleDotnet.Test.Domain.Quest;

// See qm_test_auto_complete in TestData/questSettings.xml
[Collection("Domain")]
public class PlayerAutoCompleteTaskTest
{
    private const string QuestName = "qm_test_auto_complete";

    [Fact]
    public void Player_HandleQuestsProgress_AutoCompleteTask_IsCompletedOnAnyAction()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        player.Quests.Add(quest);

        player.HandleQuestsProgress(faker.Random.String2(10));

        quest.Progress.Should().Equal(20, 0);
    }

    [Fact]
    public void Player_HandleQuestsProgress_AutoCompleteTask_OtherTasksStillRequired()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        player.Quests.Add(quest);

        player.HandleQuestsProgress("visitorHelp", "businessSendTour");
        player.CheckCompletedQuests();

        quest.Progress.Should().Equal(20, 1);
        quest.QuestType.Should().Be(QuestType.Active);
    }

    [Fact]
    public void Player_CheckCompletedQuests_AutoCompleteTaskAndOtherTasksDone_CompletesQuest()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest(name: QuestName, length: 2);
        quest.Progress[1] = 5;
        player.Quests.Add(quest);

        player.HandleQuestsProgress(faker.Random.String2(10));
        player.CheckCompletedQuests();

        quest.QuestType.Should().Be(QuestType.Completed);
    }
}
