using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

// test_bus_upgradable requires level 2 and 5 upgrade actions, then becomes test_bus_upgradable_2 (commodityReq 20) and gives 100 coins
// onUpgrade: resetUpgradeActionCount, visits = maxVisits, closeBusiness + makeHarvestable
[Collection("Database")]
public class UpgradeBuildingTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static UpgradeBuildingRequest CreateUpgradeBuildingRequest(int x, int y)
    {
        return new UpgradeBuildingRequest
        {
            Building = new BuildingUpgradeBuildingRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task UpgradeBuilding_RequirementsMet_UpgradesBusinessReadyToHarvest()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        business.SetUpgradeAction(5);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var updatedBusiness = await Context.Set<WorldObject>().FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);

        updatedBusiness.ItemName.Should().Be("test_bus_upgradable_2");
        updatedBusiness.State.Should().Be(WorldObjectState.ClosedHarvestable);
        updatedBusiness.Visits.Should().Be(20);
        updatedBusiness.UpgradeActionCount.Should().Be(0);
        updatedBusiness.CanHarvest().Should().BeTrue();
        player.Gold.Should().Be(goldBefore + 100);
    }

    [Fact]
    public async Task UpgradeBuilding_RandomUpgradeNotRolled_RollsAndGrantsToken()
    {
        var crate = Faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration, x: 10, y: 10);
        var headquarters = Faker.WorldObject(itemName: "mun_national_park_hq_2", className: BuildingClassType.Municipal, x: 20, y: 20);
        var player = Faker.Player(world: Faker.World(objects: [crate, headquarters]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await LoadPlayerAsync(player.Id);
        var storedCrate = storedPlayer.GetWorld().GetBuildingById(crate.WorldFlatId)!;

        storedCrate.ItemName.Should().Be("test_mystery_c");
        storedCrate.UpgradeItemName.Should().BeNull();
        storedPlayer.FeatureRollCounters.Should().ContainSingle(x => x.Feature == "lootTables" && x.Count == 1);
        storedPlayer.CountInventoryItem("test_mystery_c_token").Should().Be(1);
    }

    [Fact]
    public async Task UpgradeBuilding_RandomUpgradeAlreadyRolled_UsesStoredItemWithoutRolling()
    {
        var crate = Faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration, x: 10, y: 10);
        crate.SetUpgradeItemName("test_mystery_b");
        var player = Faker.Player(world: Faker.World(objects: [crate]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await LoadPlayerAsync(player.Id);
        var storedCrate = storedPlayer.GetWorld().GetBuildingById(crate.WorldFlatId)!;

        storedCrate.ItemName.Should().Be("test_mystery_b");
        storedCrate.UpgradeItemName.Should().BeNull();
        storedPlayer.FeatureRollCounters.Should().BeEmpty();
        storedPlayer.CountInventoryItem("test_mystery_b_token").Should().Be(1);
    }

    [Fact]
    public async Task UpgradeBuilding_MysteryCollectionFixedUpgrade_GrantsTokenWithoutRolling()
    {
        var crate = Faker.WorldObject(itemName: "test_mystery_fixed", className: BuildingClassType.Decoration, x: 10, y: 10);
        var player = Faker.Player(world: Faker.World(objects: [crate]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await LoadPlayerAsync(player.Id);

        storedPlayer.GetWorld().GetBuildingById(crate.WorldFlatId)!.ItemName.Should().Be("test_mystery_a");
        storedPlayer.FeatureRollCounters.Should().BeEmpty();
        storedPlayer.CountInventoryItem("test_mystery_a_token").Should().Be(1);
    }

    [Fact]
    public async Task UpgradeBuilding_MysteryCollectionLastToken_TradesCollectionIn()
    {
        var crate = Faker.WorldObject(itemName: "test_mystery_fixed", className: BuildingClassType.Decoration, x: 10, y: 10);
        var player = Faker.Player(world: Faker.World(objects: [crate]));
        player.AddItem("test_mystery_c_token");

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var goldBefore = player.Gold;
        Context.ChangeTracker.Clear();

        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await LoadPlayerAsync(player.Id);

        storedPlayer.HasItem("test_mystery_a_token").Should().BeFalse();
        storedPlayer.HasItem("test_mystery_c_token").Should().BeFalse();
        storedPlayer.CountInventoryItem("test_mystery_reward").Should().Be(1);
        storedPlayer.Gold.Should().Be(goldBefore + 500);
        storedPlayer.Collections.Should().ContainSingle(x => x.Name == "test_mystery_collection" && x.TradeIns == 1);

        var orphanTokens = await Context.Set<InventoryItem>().AsNoTracking()
            .CountAsync(x => x.Name == "test_mystery_a_token" || x.Name == "test_mystery_c_token", TestContext.Current.CancellationToken);
        orphanTokens.Should().Be(0);
    }

    private async Task<Player> LoadPlayerAsync(Guid playerId)
    {
        Context.ChangeTracker.Clear();

        return await Context.Set<Player>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Worlds)
            .ThenInclude(x => x.Objects)
            .Include(x => x.FeatureRollCounters)
            .Include(x => x.Collections)
            .Include(x => x.InventoryItems)
            .FirstAsync(x => x.Id == playerId, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task UpgradeBuilding_NotEnoughUpgradeActions_ReturnsInvalidState()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        business.SetUpgradeAction(4);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.InvalidState);
        business.ItemName.Should().Be("test_bus_upgradable");
        business.UpgradeActionCount.Should().Be(4);
    }

    [Fact]
    public async Task UpgradeBuilding_LevelTooLow_ReturnsInvalidState()
    {
        var business = Faker.WorldObject(itemName: "test_bus_upgradable", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        business.SetUpgradeAction(5);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpgradeBuilding(Context);

        var response = await handler.HandlePacket(CreateUpgradeBuildingRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.InvalidState);
        business.ItemName.Should().Be("test_bus_upgradable");
    }
}
