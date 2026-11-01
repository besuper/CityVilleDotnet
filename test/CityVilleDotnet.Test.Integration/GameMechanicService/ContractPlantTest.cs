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

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

[Collection("Database")]
public class ContractPlantTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static ContractPlantRequest CreateRequest(int objectId, string gameMode, string contractName)
    {
        return new ContractPlantRequest
        {
            ObjectId = objectId,
            GameMode = gameMode,
            ExtraData = new ContractPlantExtraData { ContractName = contractName }
        };
    }

    [Fact]
    public async Task ContractPlant_StockExchange_ChargesCoinsPlantsAndProgressesQuest()
    {
        var building = Faker.WorldObject(itemName: "test_stockexchange", className: BuildingClassType.PeepCapacityResource, state: WorldObjectState.Plowed, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetGold(2000);
        player.Quests.Add(Faker.Quest(name: "qm_test_stock_exchange", length: 3));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new ContractPlant(Context);

        var response = await handler.HandlePacket(CreateRequest(5, "GMPlant", "test_contract_stock"), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await Context.Set<Player>().AsNoTracking().Include(x => x.Quests).FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Gold.Should().Be(500);
        storedPlayer.Quests.First(x => x.Name == "qm_test_stock_exchange").Progress.Should().Equal(1, 0, 0);

        var storedBuilding = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);
        storedBuilding.State.Should().Be(WorldObjectState.Planted);
        storedBuilding.ContractName.Should().Be("test_contract_stock");
    }

    [Fact]
    public async Task ContractPlant_GameModeWithoutContractPlant_ThrowsAndKeepsBuildingPlowed()
    {
        var building = Faker.WorldObject(itemName: "test_stockexchange", className: BuildingClassType.PeepCapacityResource, state: WorldObjectState.Plowed, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetGold(2000);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new ContractPlant(Context);

        var act = () => handler.HandlePacket(CreateRequest(5, "GMPlay", "test_contract_stock"), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();

        var storedBuilding = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);
        storedBuilding.State.Should().Be(WorldObjectState.Plowed);
    }
}
