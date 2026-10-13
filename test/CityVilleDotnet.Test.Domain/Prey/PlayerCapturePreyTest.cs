using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.PreyGroup;
using CityVilleDotnet.Factory.PreyHunter;
using CityVilleDotnet.Factory.Quest;

namespace CityVilleDotnet.Test.Domain.Prey;

// See test_prey in TestData/gameSettings.xml
[Collection("Domain")]
public class PlayerCapturePreyTest
{
    private const string GroupId = "test_prey";
    private const long Now = 10_000_000;

    private static PreyGroupSetting Setting => GameSettingsManager.Instance.GetPreyGroup(GroupId)!;

    private static CityVilleDotnet.Domain.Entities.Player CreatePlayer(Faker faker, List<int> preys, params CityVilleDotnet.Domain.Entities.PreyHunter[] hunters)
    {
        var player = faker.Player();
        player.SetGold(0);
        player.PreyGroups.Add(faker.PreyGroup(groupId: GroupId, preys: preys, hunters: [..hunters]));

        return player;
    }

    [Fact]
    public void Player_CapturePrey_DefaultMode_GrantsRewards()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [1], faker.PreyHunter(position: 0), faker.PreyHunter(position: 1));

        player.CapturePrey(Setting, hubLevel: 2, preyId: 1, crewSlots: [0, 1], bought: false, Now);

        player.Gold.Should().Be(300);
        player.CountInventoryItem("test_prey_reward").Should().Be(1);
        player.CountCollectableByName("testitem1").Should().Be(1);
        player.GetNumPreysCaptured(GroupId).Should().Be(1);
    }

    [Fact]
    public void Player_CapturePrey_EveryCapturesToMaster_GrantsMasteryRewards()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [2, 2], faker.PreyHunter(position: 0), faker.PreyHunter(position: 1));

        player.CapturePrey(Setting, hubLevel: 2, preyId: 2, crewSlots: [0], bought: false, Now);
        player.Gold.Should().Be(100);

        player.CapturePrey(Setting, hubLevel: 2, preyId: 2, crewSlots: [1], bought: false, Now);
        player.Gold.Should().Be(1100);
    }

    [Fact]
    public void Player_CapturePrey_SuperheroMode_CapturesWithoutCrew()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [1]);

        player.CapturePrey(Setting, hubLevel: 3, preyId: 1, crewSlots: [], bought: false, Now);

        player.Gold.Should().Be(800);
    }

    [Fact]
    public void Player_CapturePrey_HubLevelTooLow_ThrowsInvalidState()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [3]);

        var act = () => player.CapturePrey(Setting, hubLevel: 2, preyId: 3, crewSlots: [], bought: false, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void Player_CapturePrey_UnknownPrey_ThrowsInvalidData()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [99]);

        var act = () => player.CapturePrey(Setting, hubLevel: 2, preyId: 99, crewSlots: [], bought: false, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidData);
    }

    [Fact]
    public void Player_CapturePrey_Bought_AcceptsMissingHunters()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [1], faker.PreyHunter(position: 0));

        player.CapturePrey(Setting, hubLevel: 2, preyId: 1, crewSlots: [0], bought: true, Now);

        player.GetNumPreysCaptured(GroupId).Should().Be(1);
    }

    [Fact]
    public void Player_CapturePrey_BoughtWithSleepingHunter_ThrowsInvalidState()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [1], faker.PreyHunter(position: 0, state: HunterState.Sleeping, timestamp: Now - 3600));

        var act = () => player.CapturePrey(Setting, hubLevel: 2, preyId: 1, crewSlots: [0], bought: true, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void Player_CapturePrey_ProgressesMatchingQuestTasks()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [2, 1], faker.PreyHunter(position: 0), faker.PreyHunter(position: 1), faker.PreyHunter(position: 2));
        var quest = faker.Quest(name: "qm_test_capture_prey", length: 4);
        player.Quests.Add(quest);

        player.CapturePrey(Setting, hubLevel: 2, preyId: 2, crewSlots: [0], bought: false, Now);
        player.CapturePrey(Setting, hubLevel: 2, preyId: 1, crewSlots: [1, 2], bought: false, Now);

        quest.Progress.Should().Equal(2, 1, 0, 2);
    }

    [Fact]
    public void Player_BuyPreyResource_SleepingHunter_WakesHunterAndRemovesCash()
    {
        var faker = new Faker();
        var hunter = faker.PreyHunter(position: 0, state: HunterState.Sleeping, timestamp: Now - 3600);
        var player = CreatePlayer(faker, [], hunter);
        player.SetCash(10);

        player.BuyPreyResource(Setting, position: 0, Now);

        player.Cash.Should().Be(5);
        hunter.State.Should().Be(HunterState.Patrolling);
        hunter.Timestamp.Should().Be(Now);
    }

    [Fact]
    public void Player_BuyPreyResource_PatrollingHunter_ThrowsInvalidState()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [], faker.PreyHunter(position: 0));
        player.SetCash(10);

        var act = () => player.BuyPreyResource(Setting, position: 0, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void Player_BuyPreyResource_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, [], faker.PreyHunter(position: 0, state: HunterState.Sleeping, timestamp: Now - 3600));
        player.SetCash(4);

        var act = () => player.BuyPreyResource(Setting, position: 0, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
    }
}
