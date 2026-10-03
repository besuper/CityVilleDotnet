using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class StartContractTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private static StartContractRequest CreateStartContractRequest(int id, string contractName, long? clientEnqueueTime = null)
    {
        return new StartContractRequest
        {
            Building = new BuildingStartContractRequest
            {
                Id = id,
                State = WorldObjectState.Planted,
                ContractName = contractName
            },
            ClientEnqueueTime = clientEnqueueTime
        };
    }

    [Fact]
    public async Task StartContract_PlowedPlotWithCoinContract_RemovesCoinsAndPlantsAtClientTime()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, state: WorldObjectState.Plowed, x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var clientEnqueueTime = StartTime.AddSeconds(-5).ToUnixTimeSeconds();
        var handler = new StartContract(Context);

        var response = await handler.HandlePacket(CreateStartContractRequest(plot.WorldFlatId, "plot_strawberries", clientEnqueueTime), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Gold.Should().Be(goldBefore - 15);
        plot.ContractName.Should().Be("plot_strawberries");
        plot.State.Should().Be(WorldObjectState.Planted);
        plot.PlantTime.Should().Be(clientEnqueueTime * 1000);
        plot.CanHarvest().Should().BeFalse();
    }

    [Fact]
    public async Task StartContract_NotEnoughCoins_ThrowsNotEnoughMoneyAndKeepsPlotPlowed()
    {
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, state: WorldObjectState.Plowed, x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);
        player.SetGold(14);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new StartContract(Context);

        var act = () => handler.HandlePacket(CreateStartContractRequest(plot.WorldFlatId, "plot_strawberries"), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        player.Gold.Should().Be(14);
        plot.State.Should().Be(WorldObjectState.Plowed);
        plot.ContractName.Should().BeNull();
    }

    [Fact]
    public async Task StartContract_UnknownContract_ThrowsException()
    {
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, state: WorldObjectState.Plowed, x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var handler = new StartContract(Context);

        var act = () => handler.HandlePacket(CreateStartContractRequest(plot.WorldFlatId, "unknown_contract"), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>().WithMessage("*unknown_contract*");
        player.Gold.Should().Be(goldBefore);
        plot.State.Should().Be(WorldObjectState.Plowed);
    }

    [Fact]
    public async Task StartContract_CashContract_RemovesCashInsteadOfCoins()
    {
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, state: WorldObjectState.Plowed, x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);
        player.SetCash(10);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var handler = new StartContract(Context);

        var response = await handler.HandlePacket(CreateStartContractRequest(plot.WorldFlatId, "test_crop_cash"), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(6);
        player.Gold.Should().Be(goldBefore);
        plot.ContractName.Should().Be("test_crop_cash");
    }

    [Fact]
    public async Task StartContract_CashContractNotEnoughCash_ThrowsNotEnoughMoney()
    {
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, state: WorldObjectState.Plowed, x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);
        player.SetCash(3);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new StartContract(Context);

        var act = () => handler.HandlePacket(CreateStartContractRequest(plot.WorldFlatId, "test_crop_cash"), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        player.Cash.Should().Be(3);
        plot.ContractName.Should().BeNull();
    }

    [Fact]
    public async Task StartContract_TextileContract_RemovesGoods()
    {
        var factory = Faker.WorldObject(itemName: "factory_premiumgoods", className: BuildingClassType.Factory, state: WorldObjectState.Plowed, x: 10, y: 10);
        var world = Faker.World(objects: [factory]);
        var player = Faker.Player(world: world);
        player.SetGoods(150);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var handler = new StartContract(Context);

        var response = await handler.HandlePacket(CreateStartContractRequest(factory.WorldFlatId, "test_textile_contract"), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Goods.Should().Be(50);
        player.Gold.Should().Be(goldBefore);
        factory.ContractName.Should().Be("test_textile_contract");
    }

    [Fact]
    public async Task StartContract_AlreadyPlantedPlot_ThrowsInvalidStateAndKeepsContract()
    {
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var goldBefore = player.Gold;
        var handler = new StartContract(Context);

        var act = () => handler.HandlePacket(CreateStartContractRequest(plot.WorldFlatId, "plot_corn"), player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidState);
        player.Gold.Should().Be(goldBefore);
        plot.ContractName.Should().Be("plot_strawberries");
    }
}
