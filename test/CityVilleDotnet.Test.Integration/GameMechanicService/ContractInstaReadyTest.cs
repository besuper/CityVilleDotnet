using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

[Collection("Database")]
public class ContractInstaReadyTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task ContractInstaReady_JustPlanted_ChargesCashAndGrowsContract()
    {
        // growTime 0.5 day = 11.5h, 2 cash per hour => 23
        var building = Faker.WorldObject(itemName: "test_stockexchange", className: BuildingClassType.PeepCapacityResource, state: WorldObjectState.Plowed, worldFlatId: 5);
        var player = Faker.Player(world: Faker.World(objects: [building]));
        player.SetGold(1500);
        player.SetCash(30);
        player.PlantMechanicContract(building, GameSettingsManager.Instance.GetItem("test_stockexchange")!, GameSettingsManager.Instance.GetItem("test_contract_stock")!);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var handler = new ContractInstaReady(Context);

        var response = await handler.HandlePacket(new ContractInstaReadyRequest { ObjectId = 5, GameMode = "InstaReadyClick" }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.Cash.Should().Be(7);

        var storedBuilding = await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == building.Id, TestContext.Current.CancellationToken);
        storedBuilding.State.Should().Be(WorldObjectState.Grown);
    }
}
