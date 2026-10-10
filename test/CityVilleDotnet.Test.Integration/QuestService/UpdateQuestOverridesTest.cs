using AwesomeAssertions;
using CityVilleDotnet.Api.Services.QuestService;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.GlobalTableOverride;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.QuestService;

// qm_test_global_table: task 1 declares test_global_z_table and test_global_a_table on Business
[Collection("Database")]
public class UpdateQuestOverridesTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const string QuestName = "qm_test_global_table";

    [Fact]
    public async Task UpdateQuestOverrides_DeclaredTable_IsPersisted()
    {
        var user = Faker.Player();
        user.Quests.Add(Faker.Quest(name: QuestName));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpdateQuestOverrides(Context, NullLogger<UpdateQuestOverrides>.Instance);
        var request = new UpdateQuestOverridesRequest
        {
            OverridesToAdd =
            [
                CreateOverride(1, "Business", "test_global_z_table"),
                CreateOverride(1, "Business", "test_global_a_table")
            ]
        };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var overrides = await GetOverrides(user.Id);
        overrides.Should().HaveCount(2);
        overrides.Should().OnlyContain(x => x.Keyword == "Business" && x.Source == QuestName);
        overrides.Select(x => x.Table).Should().BeEquivalentTo("test_global_z_table", "test_global_a_table");
    }

    [Fact]
    public async Task UpdateQuestOverrides_UndeclaredTable_IsRejected()
    {
        var user = Faker.Player();
        user.Quests.Add(Faker.Quest(name: QuestName));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpdateQuestOverrides(Context, NullLogger<UpdateQuestOverrides>.Instance);
        var request = new UpdateQuestOverridesRequest
        {
            OverridesToAdd = [CreateOverride(1, "Business", "test_droptable")]
        };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var overrides = await GetOverrides(user.Id);
        overrides.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateQuestOverrides_QuestNotActive_IsRejected()
    {
        var user = Faker.Player();
        user.Quests.Add(Faker.Quest(name: QuestName, questType: QuestType.Completed));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpdateQuestOverrides(Context, NullLogger<UpdateQuestOverrides>.Instance);
        var request = new UpdateQuestOverridesRequest
        {
            OverridesToAdd = [CreateOverride(1, "Business", "test_global_z_table")]
        };

        await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        var overrides = await GetOverrides(user.Id);
        overrides.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateQuestOverrides_CompletedQuest_RemovesOverride()
    {
        var user = Faker.Player();
        user.Quests.Add(Faker.Quest(name: QuestName, questType: QuestType.Completed));
        user.GlobalTableOverrides.Add(Faker.GlobalTableOverride(keyword: "Business", table: "test_global_z_table", source: QuestName));
        user.GlobalTableOverrides.Add(Faker.GlobalTableOverride(keyword: "Business", table: "test_global_a_table", source: QuestName));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpdateQuestOverrides(Context, NullLogger<UpdateQuestOverrides>.Instance);
        var request = new UpdateQuestOverridesRequest
        {
            OverridesToRemove = [CreateOverride(1, "Business", "test_global_z_table")]
        };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var overrides = await GetOverrides(user.Id);
        overrides.Should().ContainSingle(x => x.Keyword == "Business" && x.Table == "test_global_a_table" && x.Source == QuestName);
    }

    [Fact]
    public async Task UpdateQuestOverrides_AddAndRemoveInSameRequest_KeepsNothing()
    {
        var user = Faker.Player();
        user.Quests.Add(Faker.Quest(name: QuestName));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpdateQuestOverrides(Context, NullLogger<UpdateQuestOverrides>.Instance);
        var request = new UpdateQuestOverridesRequest
        {
            OverridesToAdd = [CreateOverride(1, "Business", "test_global_z_table")],
            OverridesToRemove = [CreateOverride(1, "Business", "test_global_z_table")]
        };

        await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        var overrides = await GetOverrides(user.Id);
        overrides.Should().BeEmpty();
    }

    private static QuestOverrideRequest CreateOverride(int taskId, string keyword, string table)
    {
        return new QuestOverrideRequest
        {
            QuestName = QuestName,
            TaskId = taskId,
            OverrideTable = new QuestOverrideTableRequest { Keyword = keyword, Table = table }
        };
    }

    private async Task<List<Domain.Entities.GlobalTableOverride>> GetOverrides(Guid playerId)
    {
        Context.ChangeTracker.Clear();

        return await Context.Set<Domain.Entities.Player>()
            .Where(x => x.Id == playerId)
            .SelectMany(x => x.GlobalTableOverrides)
            .ToListAsync(TestContext.Current.CancellationToken);
    }
}
