using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.GlobalTableOverride;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

// test_upgrade_table_bus pre_upgrade: grindable key (Business test_global_a_table, Community test_global_z_table) and feed key (ignored)
// test_upgrade_table_composite pre_upgrade: composite whose mechanic_inventory sub gate gives Community test_global_a_table
[Collection("Database")]
public class AddGlobalTableForUpgradeTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task AddGlobalTableForUpgrade_ByWorldFlatId_AddsGrindableTablesOfPreUpgradeGate()
    {
        var building = Faker.WorldObject(itemName: "test_upgrade_table_bus", className: BuildingClassType.Business, worldFlatId: 7);
        var user = Faker.Player(world: Faker.World(objects: [building]));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new AddGlobalTableForUpgrade(Context);

        var response = await handler.HandlePacket(new AddGlobalTableForUpgradeRequest { ObjectId = "7" }, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var overrides = await GetOverrides(user.Id);

        overrides.Select(x => (x.Keyword, x.Table, x.Source)).Should().BeEquivalentTo(new[]
        {
            ("Business", "test_global_a_table", "7"),
            ("Community", "test_global_z_table", "7")
        });
    }

    [Fact]
    public async Task AddGlobalTableForUpgrade_ByTempId_StoresWorldFlatIdAsSource()
    {
        var building = Faker.WorldObject(itemName: "test_upgrade_table_bus", className: BuildingClassType.Business, tempId: 16777217, worldFlatId: 7);
        var user = Faker.Player(world: Faker.World(objects: [building]));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new AddGlobalTableForUpgrade(Context);

        var response = await handler.HandlePacket(new AddGlobalTableForUpgradeRequest { ObjectId = "16777217" }, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var overrides = await GetOverrides(user.Id);

        overrides.Should().HaveCount(2).And.OnlyContain(x => x.Source == "7");
    }

    [Fact]
    public async Task AddGlobalTableForUpgrade_CalledTwice_DoesNotDuplicateTables()
    {
        var building = Faker.WorldObject(itemName: "test_upgrade_table_bus", className: BuildingClassType.Business, worldFlatId: 7);
        var user = Faker.Player(world: Faker.World(objects: [building]));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new AddGlobalTableForUpgrade(Context);
        var request = new AddGlobalTableForUpgradeRequest { ObjectId = "7" };

        await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();
        await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        var overrides = await GetOverrides(user.Id);

        overrides.Should().HaveCount(2);
    }

    [Fact]
    public async Task AddGlobalTableForUpgrade_CompositeGate_AddsTablesOfInventorySubGates()
    {
        var building = Faker.WorldObject(itemName: "test_upgrade_table_composite", className: BuildingClassType.Municipal, worldFlatId: 7);
        var user = Faker.Player(world: Faker.World(objects: [building]));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new AddGlobalTableForUpgrade(Context);

        var response = await handler.HandlePacket(new AddGlobalTableForUpgradeRequest { ObjectId = "7" }, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var overrides = await GetOverrides(user.Id);

        overrides.Should().ContainSingle(x => x.Keyword == "Community" && x.Table == "test_global_a_table" && x.Source == "7");
    }

    [Fact]
    public async Task AddGlobalTableForUpgrade_NoGrindableTable_AddsNothing()
    {
        var building = Faker.WorldObject(itemName: "test_gated_building", className: BuildingClassType.Municipal, worldFlatId: 7);
        var user = Faker.Player(world: Faker.World(objects: [building]));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new AddGlobalTableForUpgrade(Context);

        var response = await handler.HandlePacket(new AddGlobalTableForUpgradeRequest { ObjectId = "7" }, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var overrides = await GetOverrides(user.Id);

        overrides.Should().BeEmpty();
    }

    [Fact]
    public async Task AddGlobalTableForUpgrade_ExistingOverrides_AreKept()
    {
        var building = Faker.WorldObject(itemName: "test_upgrade_table_bus", className: BuildingClassType.Business, worldFlatId: 7);
        var user = Faker.Player(world: Faker.World(objects: [building]));
        user.GlobalTableOverrides.Add(Faker.GlobalTableOverride(keyword: "Business", table: "test_global_a_table", source: "qm_test_global_table"));
        user.GlobalTableOverrides.Add(Faker.GlobalTableOverride(keyword: "Business", table: "test_global_a_table", source: "8"));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new AddGlobalTableForUpgrade(Context);

        await handler.HandlePacket(new AddGlobalTableForUpgradeRequest { ObjectId = "7" }, user.Id, TestContext.Current.CancellationToken);

        var overrides = await GetOverrides(user.Id);

        overrides.Should().HaveCount(4);
        overrides.Where(x => x.Keyword == "Business" && x.Table == "test_global_a_table").Select(x => x.Source)
            .Should().BeEquivalentTo(new[] { "qm_test_global_table", "8", "7" });
    }

    [Fact]
    public async Task AddGlobalTableForUpgrade_OnlyAddsTablesOfTheRequestedObject()
    {
        var building = Faker.WorldObject(itemName: "test_upgrade_table_bus", className: BuildingClassType.Business, x: 10, y: 10, worldFlatId: 7);
        var composite = Faker.WorldObject(itemName: "test_upgrade_table_composite", className: BuildingClassType.Municipal, x: 20, y: 20, worldFlatId: 8);
        var user = Faker.Player(world: Faker.World(objects: [building, composite]));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new AddGlobalTableForUpgrade(Context);

        await handler.HandlePacket(new AddGlobalTableForUpgradeRequest { ObjectId = "8" }, user.Id, TestContext.Current.CancellationToken);

        var overrides = await GetOverrides(user.Id);

        overrides.Should().ContainSingle().Which.Source.Should().Be("8");
    }

    [Fact]
    public async Task AddGlobalTableForUpgrade_UnknownObject_ThrowsForceReload()
    {
        var building = Faker.WorldObject(itemName: "test_upgrade_table_bus", className: BuildingClassType.Business, worldFlatId: 7);
        var user = Faker.Player(world: Faker.World(objects: [building]));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new AddGlobalTableForUpgrade(Context);

        var act = () => handler.HandlePacket(new AddGlobalTableForUpgradeRequest { ObjectId = "99" }, user.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
        (await GetOverrides(user.Id)).Should().BeEmpty();
    }

    [Fact]
    public async Task AddGlobalTableForUpgrade_ObjectInAnotherWorld_ThrowsForceReload()
    {
        var building = Faker.WorldObject(itemName: "test_upgrade_table_bus", className: BuildingClassType.Business, worldFlatId: 7);
        var user = Faker.Player();
        user.AddWorld(Faker.World(type: WorldType.Downtown, objects: [building]));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new AddGlobalTableForUpgrade(Context);

        var act = () => handler.HandlePacket(new AddGlobalTableForUpgradeRequest { ObjectId = "7" }, user.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
    }

    [Fact]
    public async Task AddGlobalTableForUpgrade_InvalidUser_ThrowsException()
    {
        var handler = new AddGlobalTableForUpgrade(Context);

        var act = () => handler.HandlePacket(new AddGlobalTableForUpgradeRequest { ObjectId = "7" }, Guid.NewGuid(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("7.5")]
    [InlineData("99999999999999999")]
    public void AddGlobalTableForUpgrade_InvalidObjectId_FailsValidation(string objectId)
    {
        var validator = new AddGlobalTableForUpgradeRequestValidator();

        var result = validator.TestValidate(new AddGlobalTableForUpgradeRequest { ObjectId = objectId });

        result.ShouldHaveValidationErrorFor(x => x.ObjectId);
    }

    [Theory]
    [InlineData("7")]
    [InlineData("16777217")]
    public void AddGlobalTableForUpgrade_NumericObjectId_PassesValidation(string objectId)
    {
        var validator = new AddGlobalTableForUpgradeRequestValidator();

        var result = validator.TestValidate(new AddGlobalTableForUpgradeRequest { ObjectId = objectId });

        result.ShouldNotHaveAnyValidationErrors();
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
