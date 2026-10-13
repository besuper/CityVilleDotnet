using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.PreyGroup;
using CityVilleDotnet.Factory.PreyHunter;

namespace CityVilleDotnet.Test.Domain.Prey;

public class PreyGroupCaptureTest
{
    private const int SleepTime = 259200;
    private const long Now = 10_000_000;

    [Fact]
    public void PreyGroup_Capture_ExactCrew_RemovesPreyAndSleepsHunters()
    {
        var faker = new Faker();
        var group = faker.PreyGroup(preys: [1, 2], hunters: [faker.PreyHunter(position: 0), faker.PreyHunter(position: 1)]);

        var count = group.Capture(1, [0, 1], huntersRequired: 2, bought: false, SleepTime, Now);

        count.Should().Be(1);
        group.Preys.Should().Equal(2);
        group.NumPreysCaptured.Should().Be(1);
        group.Captures.Should().ContainSingle(x => x.PreyId == 1 && x.Count == 1);
        group.Hunters.Should().AllSatisfy(x =>
        {
            x.State.Should().Be(HunterState.Sleeping);
            x.Timestamp.Should().Be(Now);
        });
    }

    [Fact]
    public void PreyGroup_Capture_SamePreyTwice_IncrementsTypeCount()
    {
        var faker = new Faker();
        var group = faker.PreyGroup(preys: [2, 2]);

        group.Capture(2, [], huntersRequired: 0, bought: false, SleepTime, Now);
        var count = group.Capture(2, [], huntersRequired: 0, bought: false, SleepTime, Now);

        count.Should().Be(2);
        group.NumPreysCaptured.Should().Be(2);
        group.Captures.Should().ContainSingle(x => x.PreyId == 2 && x.Count == 2);
    }

    [Fact]
    public void PreyGroup_Capture_PreyNotInQueue_ThrowsInvalidState()
    {
        var faker = new Faker();
        var group = faker.PreyGroup(preys: [2]);

        var act = () => group.Capture(1, [], huntersRequired: 0, bought: false, SleepTime, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void PreyGroup_Capture_FreeWithMissingHunter_ThrowsInvalidData()
    {
        var faker = new Faker();
        var group = faker.PreyGroup(preys: [1], hunters: [faker.PreyHunter(position: 0), faker.PreyHunter(position: 1)]);

        var act = () => group.Capture(1, [0], huntersRequired: 2, bought: false, SleepTime, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidData);
    }

    [Fact]
    public void PreyGroup_Capture_DuplicatedSlots_CountOnce()
    {
        var faker = new Faker();
        var group = faker.PreyGroup(preys: [1], hunters: [faker.PreyHunter(position: 0), faker.PreyHunter(position: 1)]);

        var act = () => group.Capture(1, [0, 0], huntersRequired: 2, bought: false, SleepTime, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidData);
    }

    [Fact]
    public void PreyGroup_Capture_TooManyHunters_ThrowsInvalidData()
    {
        var faker = new Faker();
        var group = faker.PreyGroup(preys: [1], hunters: [faker.PreyHunter(position: 0), faker.PreyHunter(position: 1)]);

        var act = () => group.Capture(1, [0, 1], huntersRequired: 1, bought: true, SleepTime, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidData);
    }

    [Fact]
    public void PreyGroup_Capture_BoughtWithMissingHunter_Succeeds()
    {
        var faker = new Faker();
        var hunter = faker.PreyHunter(position: 0);
        var group = faker.PreyGroup(preys: [1], hunters: [hunter]);

        group.Capture(1, [0], huntersRequired: 2, bought: true, SleepTime, Now);

        group.Preys.Should().BeEmpty();
        hunter.State.Should().Be(HunterState.Sleeping);
    }

    [Fact]
    public void PreyGroup_Capture_HunterStillSleeping_ThrowsInvalidState()
    {
        var faker = new Faker();
        var group = faker.PreyGroup(preys: [1], hunters: [faker.PreyHunter(position: 0, state: HunterState.Sleeping, timestamp: Now - 3600)]);

        var act = () => group.Capture(1, [0], huntersRequired: 1, bought: false, SleepTime, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void PreyGroup_Capture_HunterSleepExpired_SleepsAgain()
    {
        var faker = new Faker();
        var hunter = faker.PreyHunter(position: 0, state: HunterState.Sleeping, timestamp: Now - SleepTime);
        var group = faker.PreyGroup(preys: [1], hunters: [hunter]);

        group.Capture(1, [0], huntersRequired: 1, bought: false, SleepTime, Now);

        hunter.State.Should().Be(HunterState.Sleeping);
        hunter.Timestamp.Should().Be(Now);
    }

    [Fact]
    public void PreyGroup_Capture_UnknownHunter_ThrowsInvalidData()
    {
        var faker = new Faker();
        var group = faker.PreyGroup(preys: [1], hunters: [faker.PreyHunter(position: 0)]);

        var act = () => group.Capture(1, [5], huntersRequired: 1, bought: false, SleepTime, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidData);
    }

    [Fact]
    public void PreyHunter_IsUsable_SleepEndsWithinTolerance_ReturnsTrue()
    {
        var faker = new Faker();
        var hunter = faker.PreyHunter(state: HunterState.Sleeping, timestamp: Now - SleepTime + 30);

        hunter.IsUsable(SleepTime, Now).Should().BeTrue();
    }

    [Fact]
    public void PreyHunter_IsUsable_SleepEndsAfterTolerance_ReturnsFalse()
    {
        var faker = new Faker();
        var hunter = faker.PreyHunter(state: HunterState.Sleeping, timestamp: Now - SleepTime + 61);

        hunter.IsUsable(SleepTime, Now).Should().BeFalse();
    }
}
