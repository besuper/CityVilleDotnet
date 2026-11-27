using AwesomeAssertions;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

// test_wonder: timer 180, sleep 1000, maintenance finishWithCash 36 for 12 keys
[Collection("Database")]
public class MaintenanceHarvestLoopTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private static MaintenanceHarvestLoopRequest Request(string operation = "finishWithCash") => new()
    {
        ObjectId = 42,
        Params = new MaintenanceHarvestLoopParamsRequest { Operation = operation }
    };

    private async Task<(Player Player, WorldObject Wonder)> SaveWonderStartedAt(int secondsAgo, int cash)
    {
        var wonder = Faker.WorldObject(itemName: "test_wonder", className: BuildingClassType.Wonder, worldFlatId: 42);
        wonder.StartHarvestLoop(GameSettingsManager.Instance.GetItem("test_wonder")!.HarvestLoopConfig!, StartTime.UtcDateTime.AddSeconds(-secondsAgo), null);
        var player = Faker.Player(world: Faker.World(objects: [wonder]));
        player.SetCash(cash);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        return (player, wonder);
    }

    [Fact]
    public async Task MaintenanceHarvestLoop_FinishWithCash_Sleeping_ChargesCashAndWakesUp()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var (player, wonder) = await SaveWonderStartedAt(500, 40);

        var response = await new MaintenanceHarvestLoop(Context).HandlePacket(Request(), player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken)).Cash.Should().Be(4);
        (await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == wonder.Id, TestContext.Current.CancellationToken)).HarvestLoopStartTime.Should().BeNull();
    }

    [Fact]
    public async Task MaintenanceHarvestLoop_FinishWithCash_Open_ThrowsWithoutCharging()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var (player, wonder) = await SaveWonderStartedAt(100, 40);

        var act = () => new MaintenanceHarvestLoop(Context).HandlePacket(Request(), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        Context.ChangeTracker.Clear();
        (await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken)).Cash.Should().Be(40);
        (await Context.Set<WorldObject>().AsNoTracking().FirstAsync(x => x.Id == wonder.Id, TestContext.Current.CancellationToken)).HarvestLoopStartTime.Should().NotBeNull();
    }

    [Fact]
    public async Task MaintenanceHarvestLoop_FinishWithCash_NotEnoughCash_Throws()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var (player, _) = await SaveWonderStartedAt(500, 10);

        var act = () => new MaintenanceHarvestLoop(Context).HandlePacket(Request(), player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData("finishWithCash", true)]
    [InlineData("useConsumables", false)]
    [InlineData("", false)]
    public void MaintenanceHarvestLoopRequestValidator_Operation_OnlyAcceptsFinishWithCash(string operation, bool isValid)
    {
        var result = new MaintenanceHarvestLoopRequestValidator().Validate(Request(operation));

        result.IsValid.Should().Be(isValid);
    }
}
