using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class CompleteTutorialTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task CompleteTutorial_MainWorld_SetsCompletedTutorialFlag()
    {
        var user = Faker.Player();

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new CompleteTutorial(Context);

        var response = await handler.HandlePacket([], user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.Flags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.IsNew.Should().BeFalse();
        player.Flags.Should().ContainSingle(x => x.Name == "completed_tutorial" && x.Value == 1);
    }

    [Fact]
    public async Task CompleteTutorial_DowntownWorld_DoesNotSetCompletedTutorialFlag()
    {
        var user = Faker.Player();
        user.AddWorld(Faker.World(type: WorldType.Downtown));
        user.SwitchWorld(WorldType.Downtown);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new CompleteTutorial(Context);

        var response = await handler.HandlePacket([], user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.Flags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.IsNew.Should().BeTrue();
        player.Flags.Should().BeEmpty();
    }
}
