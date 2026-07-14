using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.MapRect;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class FinishTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private WorldObject CreateBridgeConstructionSite(int x, int y)
    {
        var building = Faker.WorldObject(itemName: "test_bridge_expansion", className: BuildingClassType.Bridge, x: x, y: y);
        building.SetAsConstructionSite("construction_3x3_2stage", 2);

        return building;
    }

    private static FinishRequest CreateFinishRequest(int x, int y)
    {
        return new FinishRequest
        {
            Building = new BuildingFinishRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 }
            }
        };
    }

    [Fact]
    public async Task Finish_BridgeWithGrantedExpansions_AddsMapRect()
    {
        var building = CreateBridgeConstructionSite(10, 20);
        var world = Faker.World(objects: [building]);
        var user = Faker.Player(world: world);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new Finish(Context);
        var request = CreateFinishRequest(10, 20);

        var response = await handler.HandlePacket(request, user.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var mapRect = await Context.Set<MapRect>().FirstOrDefaultAsync(m => m.X == -12 && m.Y == -48);

        mapRect.Should().NotBeNull();
        mapRect.Width.Should().Be(12);
        mapRect.Height.Should().Be(12);

        var updatedObj = await Context.Set<WorldObject>().FirstAsync(o => o.Id == building.Id);

        updatedObj.ItemName.Should().Be("test_bridge_expansion");
    }

    [Fact]
    public async Task Finish_BridgeWithGrantedExpansions_IntersectingTerritory_DoesNotAddMapRect()
    {
        var existingRect = Faker.MapRect(x: -6, y: -42, width: 12, height: 12);
        var building = CreateBridgeConstructionSite(10, 20);
        var world = Faker.World(mapRects: [existingRect], objects: [building]);
        var user = Faker.Player(world: world);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new Finish(Context);
        var request = CreateFinishRequest(10, 20);

        var response = await handler.HandlePacket(request, user.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var mapRects = await Context.Set<MapRect>().ToListAsync();

        mapRects.Should().HaveCount(1);
        mapRects[0].X.Should().Be(-6);
        mapRects[0].Y.Should().Be(-42);
    }

    [Fact]
    public async Task Finish_InventoryBuildGate_ConsumesGateItems()
    {
        var building = Faker.WorldObject(itemName: "test_gated_building", className: BuildingClassType.Municipal, x: 10, y: 20);
        building.SetAsConstructionSite("construction_3x3_2stage", 2);
        var world = Faker.World(objects: [building]);
        var user = Faker.Player(world: world);
        user.AddItem("test_gate_material", 2);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new Finish(Context);
        var request = CreateFinishRequest(10, 20);

        var response = await handler.HandlePacket(request, user.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>().Include(x => x.InventoryItems).FirstAsync(x => x.Id == user.Id);
        player.HasItem("test_gate_material").Should().BeFalse();
    }

    [Fact]
    public async Task Finish_ItemWithoutGrantedExpansions_DoesNotAddMapRect()
    {
        var building = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, x: 10, y: 20);
        building.SetAsConstructionSite("construction_3x3_4stage", 4);
        var world = Faker.World(objects: [building]);
        var user = Faker.Player(world: world);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new Finish(Context);
        var request = CreateFinishRequest(10, 20);

        var response = await handler.HandlePacket(request, user.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var mapRects = await Context.Set<MapRect>().ToListAsync();

        mapRects.Should().BeEmpty();
    }
}
