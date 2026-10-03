using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class RollUpgradeItemNameTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static RollUpgradeItemNameRequest CreateRequest(int id)
    {
        return new RollUpgradeItemNameRequest
        {
            Building = new RollUpgradeItemNameBuildingRequest { Id = id }
        };
    }

    [Fact]
    public async Task RollUpgradeItemName_ByWorldFlatId_StoresRolledItemAndFeatureCounter()
    {
        var crate = Faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration, worldFlatId: 5);
        var headquarters = Faker.WorldObject(itemName: "mun_national_park_hq_2", className: BuildingClassType.Municipal, worldFlatId: 6);
        var player = Faker.Player(world: Faker.World(objects: [crate, headquarters]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new RollUpgradeItemName(Context);

        var response = await handler.HandlePacket(CreateRequest(5), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedCrate = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == crate.Id, TestContext.Current.CancellationToken);
        storedCrate.UpgradeItemName.Should().Be("test_mystery_c");
        storedCrate.ItemName.Should().Be("test_mystery_crate");

        var storedPlayer = await Context.Set<Player>().AsNoTracking().Include(x => x.FeatureRollCounters).FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.FeatureRollCounters.Should().ContainSingle(x => x.Feature == "lootTables" && x.Count == 1);
    }

    [Fact]
    public async Task RollUpgradeItemName_ByTempId_StoresRolledItem()
    {
        var crate = Faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration, tempId: 16777220, worldFlatId: 5);
        var headquarters = Faker.WorldObject(itemName: "mun_national_park_hq_2", className: BuildingClassType.Municipal, worldFlatId: 6);
        var player = Faker.Player(world: Faker.World(objects: [crate, headquarters]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new RollUpgradeItemName(Context);

        await handler.HandlePacket(CreateRequest(16777220), player.Id, TestContext.Current.CancellationToken);

        var storedCrate = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == crate.Id, TestContext.Current.CancellationToken);
        storedCrate.UpgradeItemName.Should().Be("test_mystery_c");
    }

    [Fact]
    public async Task RollUpgradeItemName_CalledTwice_RollsOnlyOnce()
    {
        var crate = Faker.WorldObject(itemName: "test_mystery_crate", className: BuildingClassType.Decoration, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [crate]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new RollUpgradeItemName(Context);

        await handler.HandlePacket(CreateRequest(5), player.Id, TestContext.Current.CancellationToken);
        var firstRoll = (await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == crate.Id, TestContext.Current.CancellationToken)).UpgradeItemName;
        Context.ChangeTracker.Clear();

        await handler.HandlePacket(CreateRequest(5), player.Id, TestContext.Current.CancellationToken);

        var storedCrate = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == crate.Id, TestContext.Current.CancellationToken);
        storedCrate.UpgradeItemName.Should().Be(firstRoll).And.BeOneOf("test_mystery_a", "test_mystery_b");

        var storedPlayer = await Context.Set<Player>().AsNoTracking().Include(x => x.FeatureRollCounters).FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.FeatureRollCounters.Should().ContainSingle(x => x.Feature == "lootTables" && x.Count == 1);
    }

    [Fact]
    public async Task RollUpgradeItemName_UnknownBuilding_ForcesReload()
    {
        var player = Faker.Player(world: Faker.World());

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new RollUpgradeItemName(Context);

        var act = () => handler.HandlePacket(CreateRequest(999), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
    }
}
