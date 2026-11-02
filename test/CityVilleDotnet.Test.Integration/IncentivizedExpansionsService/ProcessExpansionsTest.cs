using AwesomeAssertions;
using CityVilleDotnet.Api.Services.IncentivizedExpansionsService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.IncentivizedExpansionsService;

[Collection("Database")]
public class ProcessExpansionsTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task ProcessExpansions_GrantRewards_CompletesExpansionAndProgressesQuest()
    {
        var player = Faker.Player(world: Faker.World());
        player.Quests.Add(Faker.Quest(name: "qm_test_incentivized_expansion", length: 1));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        Context.ChangeTracker.Clear();

        var request = new ProcessExpansionsRequest
        {
            Operations = [new ProcessExpansionsOperation { Op = "grantRewards", Params = new ProcessExpansionsParams { IncExpId = "test_expansion_a" } }]
        };

        var response = await new ProcessExpansions(Context, NullLogger<ProcessExpansions>.Instance).HandlePacket(request, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var storedPlayer = await Context.Set<Player>().AsNoTracking()
            .Include(x => x.Quests)
            .Include(x => x.Worlds)
            .ThenInclude(x => x.IncentivizedExpansions)
            .FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        storedPlayer.GetWorld().IncentivizedExpansions.Should().ContainSingle(x => x.ExpansionId == "test_expansion_a" && x.IsCompleted);
        storedPlayer.Quests.First(x => x.Name == "qm_test_incentivized_expansion").Progress.Should().Equal(1);
    }
}
