using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.GlobalTableOverride;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;
using FluorineFx;

namespace CityVilleDotnet.Test.Domain.GlobalTableOverride;

// qm_test_global_table: task 1 declares test_global_z_table and test_global_a_table on Business, task 2 test_global_z_table on Community
// test_global_table_bus: keywords Community then Business, own table test_droptable (rollRange 99)
// test_global_a_table rollRange 1000000, test_global_z_table rollRange 0 so the roll of z is always 0
[Collection("Domain")]
public class GlobalTableOverrideTest
{
    private const string QuestName = "qm_test_global_table";
    private const string ItemName = "test_global_table_bus";
    private const int Snuid = 333;

    [Fact]
    public void Player_AddQuestTableOverride_DeclaredTable_AddsOverride()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Add(faker.Quest(name: QuestName));

        var added = player.AddQuestTableOverride(QuestName, 1, "Business", "test_global_z_table");

        added.Should().BeTrue();
        player.GlobalTableOverrides.Should().ContainSingle(x => x.Keyword == "Business" && x.Table == "test_global_z_table" && x.Source == QuestName);
    }

    [Fact]
    public void Player_AddQuestTableOverride_SameTableTwice_KeepsBothOccurrences()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Add(faker.Quest(name: QuestName));

        player.AddQuestTableOverride(QuestName, 1, "Business", "test_global_z_table");
        player.AddQuestTableOverride(QuestName, 1, "Business", "test_global_z_table");

        player.GlobalTableOverrides.Should().HaveCount(2);
    }

    [Fact]
    public void Player_AddQuestTableOverride_QuestNotActive_ReturnsFalse()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Add(faker.Quest(name: QuestName, questType: QuestType.Completed));

        var added = player.AddQuestTableOverride(QuestName, 1, "Business", "test_global_z_table");

        added.Should().BeFalse();
        player.GlobalTableOverrides.Should().BeEmpty();
    }

    [Fact]
    public void Player_AddQuestTableOverride_UnknownQuest_ReturnsFalse()
    {
        var faker = new Faker();
        var player = faker.Player();
        var quest = faker.Quest();
        player.Quests.Add(quest);

        var added = player.AddQuestTableOverride(quest.Name, 1, "Business", "test_global_z_table");

        added.Should().BeFalse();
        player.GlobalTableOverrides.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0, "Business", "test_global_z_table")] // task without override table
    [InlineData(1, "Community", "test_global_z_table")] // keyword declared by another task
    [InlineData(1, "Business", "test_droptable")] // table not declared
    [InlineData(3, "Business", "test_global_z_table")] // task out of range
    public void Player_AddQuestTableOverride_NotDeclaredByTask_ReturnsFalse(int taskId, string keyword, string table)
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Quests.Add(faker.Quest(name: QuestName));

        var added = player.AddQuestTableOverride(QuestName, taskId, keyword, table);

        added.Should().BeFalse();
        player.GlobalTableOverrides.Should().BeEmpty();
    }

    [Fact]
    public void Player_RemoveQuestTableOverride_RemovesSingleOccurrence()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.GlobalTableOverrides.Add(faker.GlobalTableOverride(keyword: "Business", table: "test_global_z_table", source: QuestName));
        player.GlobalTableOverrides.Add(faker.GlobalTableOverride(keyword: "Business", table: "test_global_z_table", source: QuestName));

        player.RemoveQuestTableOverride(QuestName, "Business", "test_global_z_table");

        player.GlobalTableOverrides.Should().ContainSingle(x => x.Keyword == "Business" && x.Table == "test_global_z_table" && x.Source == QuestName);
    }

    [Fact]
    public void Player_RemoveQuestTableOverride_OtherSource_IsKept()
    {
        var faker = new Faker();
        var player = faker.Player();
        var otherSource = faker.GlobalTableOverride(keyword: "Business", table: "test_global_z_table");
        player.GlobalTableOverrides.Add(otherSource);

        player.RemoveQuestTableOverride(QuestName, "Business", "test_global_z_table");

        player.GlobalTableOverrides.Should().ContainSingle().Which.Should().BeSameAs(otherSource);
    }

    [Fact]
    public void Player_CollectDoobersRewards_NoGlobalTable_RollsOnlyItemTable()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Snuid = Snuid;

        var secureRands = player.CollectDoobersRewards(ItemName);

        secureRands.Should().Equal(Roll(99, 1));
    }

    [Fact]
    public void Player_CollectDoobersRewards_TablesOfAKeyword_AreSortedByName()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Snuid = Snuid;
        player.Quests.Add(faker.Quest(name: QuestName));
        player.AddQuestTableOverride(QuestName, 1, "Business", "test_global_z_table");
        player.AddQuestTableOverride(QuestName, 1, "Business", "test_global_a_table");

        var secureRands = player.CollectDoobersRewards(ItemName);

        // a then z, whatever the insertion order
        secureRands.Should().Equal(Roll(99, 1), Roll(1000000, 2), 0);
    }

    [Fact]
    public void Player_CollectDoobersRewards_Tables_FollowItemKeywordsOrder()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Snuid = Snuid;
        player.Quests.Add(faker.Quest(name: QuestName));
        player.AddQuestTableOverride(QuestName, 1, "Business", "test_global_a_table");
        player.AddQuestTableOverride(QuestName, 2, "Community", "test_global_z_table");

        var secureRands = player.CollectDoobersRewards(ItemName);

        // Community (z) is the first keyword of the item, before Business (a)
        secureRands.Should().Equal(Roll(99, 1), 0, Roll(1000000, 3));
    }

    [Fact]
    public void Player_CollectDoobersRewards_SameTableFromBuildingAndQuest_RolledOnce()
    {
        var faker = new Faker();
        var provider = faker.WorldObject(itemName: "test_global_table_provider", className: BuildingClassType.Decoration);
        var player = faker.Player(world: faker.World(objects: [provider]));
        player.Snuid = Snuid;
        player.Quests.Add(faker.Quest(name: QuestName));
        player.AddQuestTableOverride(QuestName, 1, "Business", "test_global_a_table");

        var secureRands = player.CollectDoobersRewards(ItemName);

        secureRands.Should().Equal(Roll(99, 1), Roll(1000000, 2));
    }

    [Fact]
    public void Player_ToDto_NoOverride_SendsEmptyGlobalTable()
    {
        var faker = new Faker();
        var player = faker.Player();

        var dto = player.ToDto();

        dto.FeatureData["globalTable"].Should().BeOfType<ASObject>().Which.Should().BeEmpty();
    }

    [Fact]
    public void Player_ToDto_SendsGlobalTableGroupedByKeywordAndTable()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.GlobalTableOverrides.Add(faker.GlobalTableOverride(keyword: "Business", table: "test_global_z_table", source: "quest_1"));
        player.GlobalTableOverrides.Add(faker.GlobalTableOverride(keyword: "Business", table: "test_global_a_table", source: "quest_1"));
        player.GlobalTableOverrides.Add(faker.GlobalTableOverride(keyword: "Business", table: "test_global_z_table", source: "quest_2"));
        player.GlobalTableOverrides.Add(faker.GlobalTableOverride(keyword: "Community", table: "test_global_z_table", source: "quest_2"));

        var globalTable = player.ToDto().FeatureData["globalTable"].Should().BeOfType<ASObject>().Subject;

        globalTable.Should().HaveCount(2);

        var business = globalTable["Business"].Should().BeOfType<ASObject>().Subject;
        business.Should().HaveCount(2);
        business["test_global_z_table"].Should().Be("quest_1,quest_2");
        business["test_global_a_table"].Should().Be("quest_1");

        var community = globalTable["Community"].Should().BeOfType<ASObject>().Subject;
        community["test_global_z_table"].Should().Be("quest_2");
    }

    private static int Roll(int rollRange, int rollCounter)
    {
        return SecureRand.GenerateRand(0, rollRange, rollCounter, Snuid.ToString());
    }
}
