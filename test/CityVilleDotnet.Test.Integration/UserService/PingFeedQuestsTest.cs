using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.InventoryItem;
using CityVilleDotnet.Factory.MapRect;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
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

    // qm_test_lakefront_canal: tuned gate of 6 materials at level 1, consumes them all and rewards test_canal_coupon
    [Fact]
    public async Task PingFeedQuests_ItemGateComplete_CompletesQuestConsumesItemsAndGivesCoupon()
    {
        var user = Faker.Player(world: Faker.World(objects: [Faker.WorldObject()]));
        user.Quests.Add(Faker.Quest(name: "qm_test_lakefront_canal", length: 1));
        user.InventoryItems.Add(Faker.InventoryItem(itemName: "test_gate_material", amount: 6));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new PingFeedQuests(Context);

        var response = await handler.HandlePacket([], user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        Context.ChangeTracker.Clear();

        var player = await Context.Set<Player>()
            .Include(x => x.Quests)
            .Include(x => x.InventoryItems)
            .Include(x => x.Coupons)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);
        player.Quests.Should().ContainSingle(x => x.Name == "qm_test_lakefront_canal").Which.QuestType.Should().Be(QuestType.Completed);
        player.InventoryItems.Should().NotContain(x => x.Name == "test_gate_material");
        player.HasCoupon("test_canal_coupon").Should().BeTrue();
    }

    [Fact]
    public async Task PingFeedQuests_GetExpansionByCoord_SavesProgress()
    {
        var world = Faker.World(mapRects: [Faker.MapRect(x: 48, y: -84, width: 12, height: 12)], objects: [Faker.WorldObject()]);
        var user = Faker.Player(world: world);
        user.Quests.Add(Faker.Quest(name: "qm_test_lakefront_expansion", length: 2));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new PingFeedQuests(Context);

        var response = await handler.HandlePacket([], user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        Context.ChangeTracker.Clear();

        var quest = await Context.Set<Quest>().FirstAsync(x => x.Name == "qm_test_lakefront_expansion", TestContext.Current.CancellationToken);
        quest.Progress.Should().Equal(1, 0);
    }
}
