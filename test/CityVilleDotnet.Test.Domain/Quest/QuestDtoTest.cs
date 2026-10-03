using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Test.Domain.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.Quest;

[Collection("Domain")]
public class QuestDtoTest
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Quest_ToDto_ActivatedTime_IsCreationTimeInSeconds()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var quest = faker.Quest();

        quest.ToDto().ActivatedTime.Should().Be(StartTime.ToUnixTimeSeconds());
    }

    [Fact]
    public void Quest_ToDto_StartedLessThanTenSecondsAgo_IsNew()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var quest = faker.Quest();

        time.Advance(TimeSpan.FromSeconds(9));

        quest.ToDto().IsNew.Should().BeTrue();
    }

    [Fact]
    public void Quest_ToDto_StartedMoreThanTenSecondsAgo_IsNotNew()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var quest = faker.Quest();

        time.Advance(TimeSpan.FromSeconds(11));

        quest.ToDto().IsNew.Should().BeFalse();
    }

    [Fact]
    public void Quest_ToQuestComponent_OnlySendsActiveQuests()
    {
        var faker = new Faker();
        var active = faker.Quest(questType: QuestType.Active);
        var quests = new[] { active, faker.Quest(questType: QuestType.Completed), faker.Quest(questType: QuestType.Pending) }.ToList();

        var component = quests.ToQuestComponent();

        component.Should().ContainSingle().Which.Name.Should().Be(active.Name);
    }
}
