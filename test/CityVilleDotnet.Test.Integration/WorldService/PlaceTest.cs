using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.GlobalTableOverride;
using CityVilleDotnet.Factory.MapRect;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;

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
                ItemName = itemName
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
        building.TargetBuildingName.Should().Be("test_bridge_expansion");
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

    [Fact]
    public async Task Place_SkyscraperWithoutConstruction_StartsHarvestEndTimer()
    {
        var startTime = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);
        ServerUtils.TimeProvider = new FakeTimeProvider(startTime);
        var world = Faker.World();
        var user = Faker.Player(world: world);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Place(Context, NullLogger<Place>.Instance);
        var request = CreatePlaceRequest("test_skyscraper", BuildingClassType.Skyscraper, 10, 20);

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var building = await Context.Set<WorldObject>().FirstAsync(o => o.X == 10 && o.Y == 20, TestContext.Current.CancellationToken);

        building.HarvestEndTime.Should().Be(startTime.UtcDateTime.AddHours(23));
    }

    [Fact]
    public async Task Place_ConstructionWithGrindableBuildGate_AddsBuildTablesWithNewId()
    {
        var user = Faker.Player(world: Faker.World());
        user.SetGold(1000);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Place(Context, NullLogger<Place>.Instance);

        var response = await handler.HandlePacket(CreatePlaceRequest("test_build_table_bus", BuildingClassType.Municipal, 10, 20), user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var id = (response["data"] as ASObject)!["id"];
        var overrides = await GetOverrides(user.Id);

        overrides.Should().ContainSingle(x => x.Keyword == "Business" && x.Table == "test_global_a_table").Which.Source.Should().Be(id.ToString());
    }

    [Fact]
    public async Task Place_TwoConstructionSites_AddsTablesForEachSite()
    {
        var user = Faker.Player(world: Faker.World());
        user.SetGold(1000);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Place(Context, NullLogger<Place>.Instance);

        await handler.HandlePacket(CreatePlaceRequest("test_build_table_bus", BuildingClassType.Municipal, 10, 20), user.Id, TestContext.Current.CancellationToken);
        await handler.HandlePacket(CreatePlaceRequest("test_build_table_bus", BuildingClassType.Municipal, 20, 20), user.Id, TestContext.Current.CancellationToken);

        var overrides = await GetOverrides(user.Id);

        overrides.Should().HaveCount(2);
        overrides.Select(x => x.Source).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task Place_ConstructionWithoutGrindableBuildGate_AddsNoTable()
    {
        var user = Faker.Player(world: Faker.World());
        user.SetGold(1000);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Place(Context, NullLogger<Place>.Instance);

        var response = await handler.HandlePacket(CreatePlaceRequest("test_gated_building", BuildingClassType.Municipal, 10, 20), user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (await GetOverrides(user.Id)).Should().BeEmpty();
    }

    [Fact]
    public async Task Place_ExistingOverrides_AreKept()
    {
        var user = Faker.Player(world: Faker.World());
        user.SetGold(1000);
        user.GlobalTableOverrides.Add(Faker.GlobalTableOverride(keyword: "Business", table: "test_global_a_table", source: "qm_test_global_table"));

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new Place(Context, NullLogger<Place>.Instance);

        await handler.HandlePacket(CreatePlaceRequest("test_build_table_bus", BuildingClassType.Municipal, 10, 20), user.Id, TestContext.Current.CancellationToken);

        var overrides = await GetOverrides(user.Id);

        overrides.Should().HaveCount(2);
        overrides.Should().Contain(x => x.Source == "qm_test_global_table");
    }

    private async Task<List<GlobalTableOverride>> GetOverrides(Guid playerId)
    {
        Context.ChangeTracker.Clear();

        return await Context.Set<Player>()
            .Where(x => x.Id == playerId)
            .SelectMany(x => x.GlobalTableOverrides)
            .ToListAsync(TestContext.Current.CancellationToken);
    }
}
