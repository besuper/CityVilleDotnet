using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.MapRect;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class PlaceTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static PlaceRequest CreatePlaceRequest(string itemName, BuildingClassType className, int x, int y)
    {
        return new PlaceRequest
        {
            Building = new BuildingPlaceRequest
            {
                Position = new PerformActionPositionRequest { X = x, Y = y, Z = 0 },
                ClassName = className,
                State = WorldObjectState.Open,
                ItemName = itemName,
                TempId = -1
            }
        };
    }

    [Fact]
    public async Task Place_BridgeWithGrantedExpansions_AddsMapRect()
    {
        var world = Faker.World();
        var user = Faker.Player(world: world);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Place(Context, NullLogger<Place>.Instance);
        var request = CreatePlaceRequest("test_bridge_expansion", BuildingClassType.Bridge, 10, 20);

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var mapRect = await Context.Set<MapRect>().FirstOrDefaultAsync(m => m.X == -12 && m.Y == -36, TestContext.Current.CancellationToken);

        mapRect.Should().NotBeNull();
        mapRect.Width.Should().Be(12);
        mapRect.Height.Should().Be(12);

        var building = await Context.Set<WorldObject>().FirstOrDefaultAsync(o => o.X == 10 && o.Y == 20, TestContext.Current.CancellationToken);

        building.Should().NotBeNull();
        building!.TargetBuildingName.Should().Be("test_bridge_expansion");
    }

    [Fact]
    public async Task Place_BridgeWithGrantedExpansions_ExistingTerritory_DoesNotDuplicate()
    {
        var existingRect = Faker.MapRect(x: -12, y: -36, width: 12, height: 12);
        var world = Faker.World(mapRects: [existingRect]);
        var user = Faker.Player(world: world);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Place(Context, NullLogger<Place>.Instance);
        var request = CreatePlaceRequest("test_bridge_expansion", BuildingClassType.Bridge, 10, 20);

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var mapRects = await Context.Set<MapRect>().Where(m => m.X == -12 && m.Y == -36).ToListAsync(TestContext.Current.CancellationToken);

        mapRects.Should().HaveCount(1);
    }

    [Fact]
    public async Task Place_ItemWithoutGrantedExpansions_DoesNotAddMapRect()
    {
        var world = Faker.World();
        var user = Faker.Player(world: world);
        user.SetGold(1000);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Place(Context, NullLogger<Place>.Instance);
        var request = CreatePlaceRequest("deco_tree", BuildingClassType.Decoration, 10, 20);

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var mapRects = await Context.Set<MapRect>().ToListAsync(TestContext.Current.CancellationToken);

        mapRects.Should().BeEmpty();
    }

    [Fact]
    public async Task Place_OtherMapOwner_ReturnsForceReloadAndDoesNotPlace()
    {
        var world = Faker.World();
        var user = Faker.Player(world: world);
        user.SetGold(1000);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Place(Context, NullLogger<Place>.Instance);
        var request = CreatePlaceRequest("bus_bakery", BuildingClassType.Business, 10, 20);
        request.Params = [new PlaceParamsRequest { MapOwner = user.Snuid + 1 }];

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be((int)GameErrorType.ForceReload);

        var building = await Context.Set<WorldObject>().FirstOrDefaultAsync(o => o.X == 10 && o.Y == 20, TestContext.Current.CancellationToken);

        building.Should().BeNull();
    }
}
