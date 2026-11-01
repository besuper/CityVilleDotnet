using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

[Collection("Database")]
public class ContractHarvestTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private Player CreatePlayerWithInvestment(WorldObject building)
    {
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetGold(1500);
        player.PlantMechanicContract(building, GameSettingsManager.Instance.GetItem("test_stockexchange")!, GameSettingsManager.Instance.GetItem("test_contract_stock")!);

        return player;
    }

    [Fact]
    public async Task ContractHarvest_GrownInvestment_PaysReturnsConsumesSharesAndProgressesQuests()
    {
        // firstTime 300 * 1.5 + 2 shares * 10
        var building = Faker.WorldObject(itemName: "test_stockexchange", className: BuildingClassType.PeepCapacityResource, state: WorldObjectState.Plowed, worldFlatId: 5);
        var player = CreatePlayerWithInvestment(building);
        building.SetFullGrown();
        player.AddItem("test_contract_stock_share", 2);
        player.Quests.Add(Faker.Quest(name: "qm_test_stock_exchange", length: 3));
        var energyBefore = player.Energy;

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new ContractHarvest(Context);

        var response = await handler.HandlePacket(new ContractHarvestRequest { ObjectId = 5, GameMode = "GMPlay" }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await Context.Set<Player>().AsNoTracking()
            .Include(x => x.Quests)
            .Include(x => x.InventoryItems)
            .FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Gold.Should().Be(470);
        storedPlayer.Energy.Should().Be(energyBefore - 1);
        storedPlayer.CountInventoryItem("test_contract_stock_share").Should().Be(0);
        storedPlayer.Quests.First(x => x.Name == "qm_test_stock_exchange").Progress.Should().Equal(0, 1, 1);

        var storedBuilding = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);
        storedBuilding.State.Should().Be(WorldObjectState.Plowed);
        storedBuilding.ContractName.Should().BeNull();
        storedBuilding.InvestmentReturns.Should().Be(470);
        storedBuilding.InvestmentCount.Should().Be(1);
    }

    [Fact]
    public async Task ContractHarvest_NotGrown_ThrowsAndKeepsContract()
    {
        var building = Faker.WorldObject(itemName: "test_stockexchange", className: BuildingClassType.PeepCapacityResource, state: WorldObjectState.Plowed, worldFlatId: 5);
        var player = CreatePlayerWithInvestment(building);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new ContractHarvest(Context);

        var act = () => handler.HandlePacket(new ContractHarvestRequest { ObjectId = 5, GameMode = "GMPlay" }, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();

        var storedBuilding = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);
        storedBuilding.ContractName.Should().Be("test_contract_stock");
        storedBuilding.InvestmentCount.Should().Be(0);
    }
}
