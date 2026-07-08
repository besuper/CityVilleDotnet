using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

[Collection("Database")]
public class RemodelTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task Remodel_CatalogPurchase_DeductsCoinsAndStartsRemodel()
    {
        var residence = Faker.WorldObject(itemName: "test_res_base", className: BuildingClassType.Residence, worldFlatId: 5);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var goldBefore = player.Gold;
        var handler = new Remodel(Context);
        var request = new RemodelRequest
        {
            ObjectId = 5,
            GameMode = "catalogPurchase",
            ExtraData = new Dictionary<string, object> { ["itemName"] = "test_res_skin" }
        };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        player.Gold.Should().Be(goldBefore - 500);
        residence.RemodelItemName.Should().Be("test_res_skin");
        residence.RemodelBuilds.Should().Be(0);
        residence.ItemName.Should().Be("test_res_base");
    }

    [Fact]
    public async Task Remodel_CatalogPurchase_PremiumSkin_DeductsCash()
    {
        var residence = Faker.WorldObject(itemName: "test_res_base", className: BuildingClassType.Residence, worldFlatId: 5);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var cashBefore = player.Cash;
        var handler = new Remodel(Context);
        var request = new RemodelRequest
        {
            ObjectId = 5,
            GameMode = "catalogPurchase",
            ExtraData = new Dictionary<string, object> { ["itemName"] = "test_res_skin_premium" }
        };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        player.Cash.Should().Be(cashBefore - 5);
        residence.RemodelItemName.Should().Be("test_res_skin_premium");
    }

    [Fact]
    public async Task Remodel_CatalogPurchase_UnknownSkin_ReturnsInvalidData()
    {
        var residence = Faker.WorldObject(itemName: "test_res_base", className: BuildingClassType.Residence, worldFlatId: 5);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var goldBefore = player.Gold;
        var handler = new Remodel(Context);
        var request = new RemodelRequest
        {
            ObjectId = 5,
            GameMode = "catalogPurchase",
            ExtraData = new Dictionary<string, object> { ["itemName"] = "deco_tree" }
        };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be((int)GameErrorType.InvalidData);
        player.Gold.Should().Be(goldBefore);
        residence.RemodelItemName.Should().BeNull();
    }

    [Fact]
    public async Task Remodel_Click_DeductsEnergyAndIncrementsBuilds()
    {
        var residence = Faker.WorldObject(itemName: "test_res_base", className: BuildingClassType.Residence, worldFlatId: 5);
        residence.StartRemodel("test_res_skin");
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var energyBefore = player.Energy;
        var handler = new Remodel(Context);
        var request = new RemodelRequest { ObjectId = 5, GameMode = "GMRemodel" };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        player.Energy.Should().Be(energyBefore - 1);
        residence.RemodelBuilds.Should().Be(1);
        residence.ItemName.Should().Be("test_res_base");
    }

    [Fact]
    public async Task Remodel_Click_GateReached_FinishesRemodel()
    {
        var residence = Faker.WorldObject(itemName: "test_res_base", className: BuildingClassType.Residence, worldFlatId: 5);
        residence.StartRemodel("test_res_skin");
        residence.AddRemodelBuild();
        residence.AddRemodelBuild();
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var xpBefore = player.Xp;
        var handler = new Remodel(Context);
        var request = new RemodelRequest { ObjectId = 5, GameMode = "GMRemodel" };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        residence.ItemName.Should().Be("test_res_skin");
        residence.RemodelItemName.Should().BeNull();
        residence.RemodelBuilds.Should().BeNull();
        player.Xp.Should().Be(xpBefore + 4);
        world.Population.Should().Be(30);
    }

    [Fact]
    public async Task Remodel_Click_NotRemodeling_ReturnsInvalidState()
    {
        var residence = Faker.WorldObject(itemName: "test_res_base", className: BuildingClassType.Residence, worldFlatId: 5);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);
        player.SetLevel(2);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var energyBefore = player.Energy;
        var handler = new Remodel(Context);
        var request = new RemodelRequest { ObjectId = 5, GameMode = "GMRemodel" };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be((int)GameErrorType.InvalidState);
        player.Energy.Should().Be(energyBefore);
    }

    [Fact]
    public async Task Remodel_LevelTooLow_ReturnsInvalidState()
    {
        var residence = Faker.WorldObject(itemName: "test_res_base", className: BuildingClassType.Residence, worldFlatId: 5);
        var world = Faker.World(objects: [residence]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new Remodel(Context);
        var request = new RemodelRequest
        {
            ObjectId = 5,
            GameMode = "catalogPurchase",
            ExtraData = new Dictionary<string, object> { ["itemName"] = "test_res_skin" }
        };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be((int)GameErrorType.InvalidState);
        residence.RemodelItemName.Should().BeNull();
    }
}
