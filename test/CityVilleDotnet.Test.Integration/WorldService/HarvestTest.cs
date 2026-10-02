using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Franchise;
using CityVilleDotnet.Factory.FranchiseLocation;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class HarvestTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const long OneHourMs = 3_600_000;

    private static HarvestRequest CreateHarvestRequest(int x, int y)
    {
        return new HarvestRequest
        {
            Building = new BuildingHarvestRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task Harvest_ResidenceNotGrown_ThrowsException()
    {
        var residence = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime(), x: 10, y: 10);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var act = () => handler.HandlePacket(CreateHarvestRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>().WithMessage("*not harvestable*");
    }

    [Fact]
    public async Task Harvest_GrownResidence_ReturnsCoinYieldAndReplants()
    {
        var residence = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - OneHourMs, x: 10, y: 10);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var timeBefore = ServerUtils.GetCurrentTime();
        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(CreateHarvestRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var data = response["data"] as ASObject;

        data.Should().NotBeNull();
        data!["retCoinYield"].Should().Be(20);

        var updatedResidence = await Context.Set<WorldObject>().FirstAsync(x => x.Id == residence.Id, TestContext.Current.CancellationToken);

        updatedResidence.State.Should().Be(WorldObjectState.Planted);
        updatedResidence.PlantTime.Should().BeGreaterThanOrEqualTo(timeBefore);
    }

    [Fact]
    public async Task Harvest_WithClientEnqueueTime_ReplantsAtClientTime()
    {
        var startTime = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);
        ServerUtils.TimeProvider = new FakeTimeProvider(startTime);
        var residence = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - OneHourMs, x: 10, y: 10);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var clientEnqueueTime = startTime.AddSeconds(-20).ToUnixTimeSeconds();
        var request = CreateHarvestRequest(10, 10);
        request.ClientEnqueueTime = clientEnqueueTime;

        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var updatedResidence = await Context.Set<WorldObject>().FirstAsync(x => x.Id == residence.Id, TestContext.Current.CancellationToken);

        updatedResidence.State.Should().Be(WorldObjectState.Planted);
        updatedResidence.PlantTime.Should().Be(clientEnqueueTime * 1000);
    }

    [Fact]
    public async Task Harvest_GrownPlot_PlowsClearsContractAndIncrementsMastery()
    {
        var plot = Faker.WorldObject(itemName: "plot_crop", className: BuildingClassType.Plot, contractName: "plot_strawberries", state: WorldObjectState.Grown, x: 10, y: 10);
        var world = Faker.World(objects: [plot]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(CreateHarvestRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var data = response["data"] as ASObject;

        data!["retCoinYield"].Should().Be(50);

        var updatedPlot = await Context.Set<WorldObject>().FirstAsync(x => x.Id == plot.Id, TestContext.Current.CancellationToken);

        updatedPlot.State.Should().Be(WorldObjectState.Plowed);
        updatedPlot.ContractName.Should().BeNull();

        player.Masteries.Should().ContainSingle(x => x.ItemName == "plot_strawberries" && x.Count == 1);
    }

    [Fact]
    public async Task Harvest_ClosedHarvestableBusiness_ClosesWithoutEnergyCost()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.ClosedHarvestable, x: 10, y: 10);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var energyBefore = player.Energy;
        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(CreateHarvestRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var updatedBusiness = await Context.Set<WorldObject>().FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);

        updatedBusiness.State.Should().Be(WorldObjectState.Closed);
        updatedBusiness.Visits.Should().Be(0);
        player.Energy.Should().Be(energyBefore);
    }

    [Fact]
    public async Task Harvest_OpenBusiness_ThrowsException()
    {
        var business = Faker.WorldObject(itemName: "test_bus_goods", className: BuildingClassType.Business, state: WorldObjectState.Open, x: 10, y: 10);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var act = () => handler.HandlePacket(CreateHarvestRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>().WithMessage("*not harvestable*");
    }

    [Fact]
    public async Task Harvest_ClosedHarvestableBusinessWithEnergyCost_RemovesEnergy()
    {
        var business = Faker.WorldObject(itemName: "test_bus_energy", className: BuildingClassType.Business, state: WorldObjectState.ClosedHarvestable, x: 10, y: 10);
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var energyBefore = player.Energy;
        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(CreateHarvestRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Energy.Should().Be(energyBefore - 1);
    }

    // Business.as only charges harvestEnergyCost when the business is not franchise supplied, and a supplied franchise is harvestable in any state
    [Fact]
    public async Task Harvest_FranchiseSuppliedBusiness_NoEnergyCostAndConsumesCommodity()
    {
        var location = Faker.FranchiseLocation(commodityLeft: 10);
        var owner = Faker.Player();
        owner.Franchises.Add(Faker.Franchise(franchiseType: "test_bus_energy", locations: [location]));

        await Context.AddAsync(owner, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var business = Faker.WorldObject(itemName: "test_bus_energy", className: BuildingClassType.Business, state: WorldObjectState.Closed, x: 10, y: 10);
        business.SetFranchiseLocation(location, owner.Snuid.ToString());
        var world = Faker.World(objects: [business]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var energyBefore = player.Energy;
        var handler = new Harvest(Context, NullLogger<HarvestRequest>.Instance);

        var response = await handler.HandlePacket(CreateHarvestRequest(10, 10), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        player.Energy.Should().Be(energyBefore);

        var updatedLocation = await Context.Set<FranchiseLocation>().FirstAsync(x => x.Id == location.Id, TestContext.Current.CancellationToken);

        updatedLocation.CommodityLeft.Should().Be(0);
        updatedLocation.CustomersServed.Should().Be(1);

        var updatedBusiness = await Context.Set<WorldObject>().FirstAsync(x => x.Id == business.Id, TestContext.Current.CancellationToken);

        updatedBusiness.State.Should().Be(WorldObjectState.Closed);
    }
}
