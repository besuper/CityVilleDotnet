using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

[Collection("Database")]
public class HarvestStateTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task HarvestState_BridgeWithPopulationItems_DistributesPopulationToResidences()
    {
        var bridge = Faker.WorldObject(itemName: "test_bridge", className: BuildingClassType.Bridge, x: 10, y: 10, worldFlatId: 5);
        var residence = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, x: 20, y: 20, worldFlatId: 10);
        var world = Faker.World(objects: [bridge, residence]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var startingEnergy = player.Energy;

        var handler = new HarvestState(Context, new ConfigurationBuilder().Build());
        var request = new HarvestStateRequest { ObjectId = 5 };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        player.Energy.Should().Be(startingEnergy - 5);

        var updatedResidence = await Context.Set<WorldObject>()
            .Include(x => x.MechanicCounters)
            .FirstAsync(x => x.Id == residence.Id, TestContext.Current.CancellationToken);

        updatedResidence.GetBonusPopulation().Should().Be(10);
        player.GetWorld().GetCurrentPopulation().Should().Be(10);

        var inventoryItem = await Context.Set<InventoryItem>().FirstOrDefaultAsync(x => x.Name == "test_population_add", TestContext.Current.CancellationToken);
        inventoryItem.Should().BeNull();
    }

    [Fact]
    public async Task HarvestState_KeywordQuest_ProgressesHarvestMechanicByKeywords()
    {
        var bridge = Faker.WorldObject(itemName: "test_bridge", className: BuildingClassType.Bridge, x: 10, y: 10, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [bridge]));
        var quest = Faker.Quest(name: "qm_test_keywords", length: 3);
        player.Quests.Add(quest);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new HarvestState(Context, new ConfigurationBuilder().Build());

        await handler.HandlePacket(new HarvestStateRequest { ObjectId = 5 }, player.Id, TestContext.Current.CancellationToken);

        quest.Progress.Should().Equal(1, 0, 0);
    }

    [Fact]
    public async Task HarvestState_NoResidence_PopulationIsNotDistributed()
    {
        var bridge = Faker.WorldObject(itemName: "test_bridge", className: BuildingClassType.Bridge, x: 10, y: 10, worldFlatId: 5);
        var world = Faker.World(objects: [bridge]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new HarvestState(Context, new ConfigurationBuilder().Build());
        var request = new HarvestStateRequest { ObjectId = 5 };

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        player.GetWorld().GetCurrentPopulation().Should().Be(0);

        var inventoryItem = await Context.Set<InventoryItem>().FirstOrDefaultAsync(x => x.Name == "test_population_add", TestContext.Current.CancellationToken);
        inventoryItem.Should().BeNull();
    }

    // test_supply_mall supplied with test_mall_shop_a (10 customers, 1 energy) and test_mall_shop_b (20 customers, 2 energy), both paying 5 coins per customer
    private async Task<(Player Player, WorldObject Mall)> SeedSuppliedMall(Quest? quest = null)
    {
        var mall = Faker.WorldObject(itemName: "test_supply_mall", className: BuildingClassType.Mall, x: 1, y: 1, worldFlatId: 1);
        var shopA = Faker.WorldObject(itemName: "test_mall_shop_a", className: BuildingClassType.Business, worldFlatId: 10);
        var shopB = Faker.WorldObject(itemName: "test_mall_shop_b", className: BuildingClassType.Business, worldFlatId: 11);
        shopA.StoreIn(1, 0);
        shopB.StoreIn(1, 1);
        mall.OpenStorageSupply([shopA, shopB]);
        mall.UpdateVisits(30);
        var player = Faker.Player(world: Faker.World(objects: [mall, shopA, shopB]));

        if (quest is not null)
        {
            player.Quests.Add(quest);
        }

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        return (player, mall);
    }

    [Fact]
    public async Task HarvestState_SuppliedMall_PaysSuppliedShopsAndEnergyThenResetsMall()
    {
        var (player, mall) = await SeedSuppliedMall();

        var response = await new HarvestState(Context, new ConfigurationBuilder().Build()).HandlePacket(new HarvestStateRequest { ObjectId = 1 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        Context.ChangeTracker.Clear();
        var storedPlayer = await Context.Set<Player>().AsNoTracking().SingleAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Gold.Should().Be(player.Gold + 5 * 10 + 5 * 20);
        storedPlayer.Energy.Should().Be(player.Energy - (1 + 1 + 2));
        var storedMall = await Context.Set<WorldObject>().AsNoTracking().Include(x => x.SupplyResources).SingleAsync(x => x.Id == mall.Id, TestContext.Current.CancellationToken);
        storedMall.IsStorageSupplied().Should().BeFalse();
        storedMall.State.Should().Be(WorldObjectState.Static);
        storedMall.Visits.Should().Be(0);
    }

    [Fact]
    public async Task HarvestState_SuppliedMall_ProgressesHarvestQuestTasks()
    {
        var (player, _) = await SeedSuppliedMall(Faker.Quest(name: "qm_test_mall_harvest", length: 5));

        await new HarvestState(Context, new ConfigurationBuilder().Build()).HandlePacket(new HarvestStateRequest { ObjectId = 1 }, player.Id, TestContext.Current.CancellationToken);

        Context.ChangeTracker.Clear();
        var quest = await Context.Set<Player>().AsNoTracking().Where(x => x.Id == player.Id).SelectMany(x => x.Quests).SingleAsync(x => x.Name == "qm_test_mall_harvest", TestContext.Current.CancellationToken);
        quest.Progress.Should().Equal(1, 1, 1, 1, 1);
    }
}
