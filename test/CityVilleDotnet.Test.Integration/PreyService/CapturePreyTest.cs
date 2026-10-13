using AwesomeAssertions;
using CityVilleDotnet.Api.Services.PreyService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.PreyGroup;
using CityVilleDotnet.Factory.PreyHunter;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.PreyService;

// See test_prey in TestData/gameSettings.xml
[Collection("Database")]
public class CapturePreyTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const string GroupId = "test_prey";

    private async Task<Player> SeedPlayerAsync(int cash = 0)
    {
        var hub = Faker.WorldObject(itemName: "test_prey_hub_2", className: BuildingClassType.Municipal, x: 10, y: 10);
        var player = Faker.Player(world: Faker.World(objects: [hub]));
        player.SetGold(0);
        player.SetCash(cash);
        player.PreyGroups.Add(Faker.PreyGroup(groupId: GroupId, preys: [1], hunters: [Faker.PreyHunter(position: 0), Faker.PreyHunter(position: 1)]));
        player.Quests.Add(Faker.Quest(name: "qm_test_capture_prey", length: 4));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        return player;
    }

    private async Task<Player> LoadPlayerAsync(Guid playerId)
    {
        Context.ChangeTracker.Clear();

        return await Context.Set<Player>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.PreyGroups)
            .ThenInclude(x => x.Hunters)
            .Include(x => x.PreyGroups)
            .ThenInclude(x => x.Captures)
            .Include(x => x.Quests)
            .Include(x => x.InventoryItems)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .FirstAsync(x => x.Id == playerId, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task CapturePrey_FullCrew_PersistsCaptureRewardsAndQuestProgress()
    {
        var player = await SeedPlayerAsync();
        var handler = new CapturePrey(Context);

        var response = await handler.HandlePacket(new CapturePreyRequest { GroupId = GroupId, PreyId = 1, CrewSlots = [0, 1] }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var stored = await LoadPlayerAsync(player.Id);
        var group = stored.PreyGroups.Should().ContainSingle().Subject;

        group.Preys.Should().BeEmpty();
        group.NumPreysCaptured.Should().Be(1);
        group.Captures.Should().ContainSingle(x => x.PreyId == 1 && x.Count == 1);
        group.Hunters.Should().AllSatisfy(x => x.State.Should().Be(HunterState.Sleeping));
        stored.Gold.Should().Be(300);
        stored.CountInventoryItem("test_prey_reward").Should().Be(1);
        stored.CountCollectableByName("testitem1").Should().Be(1);
        stored.Quests.Should().ContainSingle(x => x.Name == "qm_test_capture_prey").Which.Progress.Should().Equal(1, 0, 0, 1);
    }

    [Fact]
    public async Task CapturePrey_MissingHunter_ThrowsInvalidData()
    {
        var player = await SeedPlayerAsync();
        var handler = new CapturePrey(Context);

        var act = () => handler.HandlePacket(new CapturePreyRequest { GroupId = GroupId, PreyId = 1, CrewSlots = [0] }, player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidData);
    }

    [Fact]
    public async Task CapturePrey_UnknownGroup_ThrowsInvalidData()
    {
        var player = await SeedPlayerAsync();
        var handler = new CapturePrey(Context);

        var act = () => handler.HandlePacket(new CapturePreyRequest { GroupId = "test_unknown_prey", PreyId = 1, CrewSlots = [0, 1] }, player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidData);
    }

    [Fact]
    public async Task BuyCapturePrey_MissingHunter_RemovesCashAndCaptures()
    {
        var player = await SeedPlayerAsync(cash: 50);
        var handler = new BuyCapturePrey(Context);

        var response = await handler.HandlePacket(new BuyCapturePreyRequest { GroupId = GroupId, PreyId = 1, CrewSlots = [0] }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var stored = await LoadPlayerAsync(player.Id);
        var group = stored.PreyGroups.Should().ContainSingle().Subject;

        stored.Cash.Should().Be(15);
        group.Preys.Should().BeEmpty();
        group.NumPreysCaptured.Should().Be(1);
        group.Hunters.Single(x => x.Position == 0).State.Should().Be(HunterState.Sleeping);
        group.Hunters.Single(x => x.Position == 1).State.Should().Be(HunterState.Patrolling);
    }

    [Fact]
    public async Task BuyCapturePrey_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var player = await SeedPlayerAsync(cash: 34);
        var handler = new BuyCapturePrey(Context);

        var act = () => handler.HandlePacket(new BuyCapturePreyRequest { GroupId = GroupId, PreyId = 1, CrewSlots = [0] }, player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
    }
}
