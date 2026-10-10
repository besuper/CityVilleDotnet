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
using FluentValidation.TestHelper;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

// test_mun_cash_upgradable (cap 100) upgrades to test_mun_cash_upgradable_2 (cap 300) for 80 cash and gives 100 coins
[Collection("Database")]
public class BuyUpgradeBuildingsTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static BuyUpgradeBuildingsRequest CreateRequest(params int[] ids)
    {
        return new BuyUpgradeBuildingsRequest { Params = [ids] };
    }

    private async Task<Player> LoadPlayerAsync(Guid playerId)
    {
        Context.ChangeTracker.Clear();

        return await Context.Set<Player>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Worlds)
            .ThenInclude(x => x.Objects)
            .FirstAsync(x => x.Id == playerId, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task BuyUpgradeBuildings_SingleBuilding_ChargesCashAndUpgrades()
    {
        var building = Faker.WorldObject(itemName: "test_mun_cash_upgradable", className: BuildingClassType.Municipal, worldFlatId: 5);
        building.SetUpgradeAction(2);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var goldBefore = player.Gold;
        Context.ChangeTracker.Clear();

        var handler = new BuyUpgradeBuildings(Context);

        var response = await handler.HandlePacket(CreateRequest(5), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var data = response["data"].Should().BeAssignableTo<List<object>>().Subject;
        data.Should().ContainSingle();
        ((ASObject)data[0])["id"].Should().Be(5);

        var storedPlayer = await LoadPlayerAsync(player.Id);
        var storedBuilding = storedPlayer.GetWorld().GetBuildingById(5)!;

        storedPlayer.Cash.Should().Be(20);
        storedPlayer.Gold.Should().Be(goldBefore + 100);
        storedBuilding.ItemName.Should().Be("test_mun_cash_upgradable_2");
        storedBuilding.UpgradeActionCount.Should().Be(0);
        storedPlayer.GetWorld().PopulationCap.Should().Be(300);
    }

    [Fact]
    public async Task BuyUpgradeBuildings_SeveralBuildings_ChargesCashOnceAndUpgradesAll()
    {
        var first = Faker.WorldObject(itemName: "test_mun_cash_upgradable", className: BuildingClassType.Municipal, worldFlatId: 5);
        var second = Faker.WorldObject(itemName: "test_mun_cash_upgradable", className: BuildingClassType.Municipal, worldFlatId: 6);
        var player = Faker.Player(world: Faker.World(objects: [first, second]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var goldBefore = player.Gold;
        Context.ChangeTracker.Clear();

        var handler = new BuyUpgradeBuildings(Context);

        var response = await handler.HandlePacket(CreateRequest(5, 6), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var data = response["data"].Should().BeAssignableTo<List<object>>().Subject;
        data.Select(x => ((ASObject)x)["id"]).Should().Equal(5, 6);

        var storedPlayer = await LoadPlayerAsync(player.Id);
        var world = storedPlayer.GetWorld();

        storedPlayer.Cash.Should().Be(20);
        storedPlayer.Gold.Should().Be(goldBefore + 200);
        world.GetBuildingById(5)!.ItemName.Should().Be("test_mun_cash_upgradable_2");
        world.GetBuildingById(6)!.ItemName.Should().Be("test_mun_cash_upgradable_2");
        world.PopulationCap.Should().Be(600);
    }

    [Fact]
    public async Task BuyUpgradeBuildings_ByTempId_UpgradesAndReturnsWorldFlatId()
    {
        var building = Faker.WorldObject(itemName: "test_mun_cash_upgradable", className: BuildingClassType.Municipal, tempId: 16777220, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyUpgradeBuildings(Context);

        var response = await handler.HandlePacket(CreateRequest(16777220), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var data = response["data"].Should().BeAssignableTo<List<object>>().Subject;
        ((ASObject)data.Single())["id"].Should().Be(5);

        var storedPlayer = await LoadPlayerAsync(player.Id);
        storedPlayer.GetWorld().GetBuildingById(5)!.ItemName.Should().Be("test_mun_cash_upgradable_2");
    }

    [Fact]
    public async Task BuyUpgradeBuildings_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var building = Faker.WorldObject(itemName: "test_mun_cash_upgradable", className: BuildingClassType.Municipal, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(79);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyUpgradeBuildings(Context);

        var act = () => handler.HandlePacket(CreateRequest(5), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);

        var storedPlayer = await LoadPlayerAsync(player.Id);
        storedPlayer.Cash.Should().Be(79);
        storedPlayer.GetWorld().GetBuildingById(5)!.ItemName.Should().Be("test_mun_cash_upgradable");
    }

    [Fact]
    public async Task BuyUpgradeBuildings_UpgradeWithoutCashCost_ReturnsInvalidState()
    {
        var building = Faker.WorldObject(itemName: "test_mun_upgradable", className: BuildingClassType.Municipal, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyUpgradeBuildings(Context);

        var response = await handler.HandlePacket(CreateRequest(5), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.InvalidState);

        var storedPlayer = await LoadPlayerAsync(player.Id);
        storedPlayer.Cash.Should().Be(100);
        storedPlayer.GetWorld().GetBuildingById(5)!.ItemName.Should().Be("test_mun_upgradable");
    }

    [Fact]
    public async Task BuyUpgradeBuildings_UnknownBuilding_ThrowsForceReload()
    {
        var player = Faker.Player(world: Faker.World(objects: []));
        player.SetCash(100);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new BuyUpgradeBuildings(Context);

        var act = () => handler.HandlePacket(CreateRequest(5), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.ForceReload);
    }

    [Fact]
    public async Task BuyUpgradeBuildings_InvalidPlayer_ThrowsException()
    {
        var handler = new BuyUpgradeBuildings(Context);

        var act = () => handler.HandlePacket(CreateRequest(5), Guid.NewGuid(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>().WithMessage("Player not found");
    }

    [Fact]
    public void BuyUpgradeBuildings_EmptyParams_FailsValidation()
    {
        var validator = new BuyUpgradeBuildingsRequestValidator();

        var result = validator.TestValidate(new BuyUpgradeBuildingsRequest { Params = [] });

        result.ShouldHaveValidationErrorFor(x => x.Params);
    }

    [Fact]
    public void BuyUpgradeBuildings_EmptyIds_FailsValidation()
    {
        var validator = new BuyUpgradeBuildingsRequestValidator();

        var result = validator.TestValidate(CreateRequest());

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void BuyUpgradeBuildings_TooManyIds_FailsValidation()
    {
        var validator = new BuyUpgradeBuildingsRequestValidator();

        var result = validator.TestValidate(CreateRequest(Enumerable.Range(1, 51).ToArray()));

        result.IsValid.Should().BeFalse();
    }
}
