using AwesomeAssertions;
using CityVilleDotnet.Api.Services.ViralAckService;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.ViralAckService;

[Collection("Database")]
public class SeenTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const int TargetObjectId = 42;

    private async Task<(Player Author, Player FirstHelper, Player SecondHelper)> SeedHelpedBuildingPost()
    {
        var author = Faker.Player();
        var firstHelper = Faker.Player();
        var secondHelper = Faker.Player();

        await Context.AddAsync(author, TestContext.Current.CancellationToken);
        await Context.AddAsync(firstHelper, TestContext.Current.CancellationToken);
        await Context.AddAsync(secondHelper, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var now = ServerUtils.TimeProvider.GetUtcNow().UtcDateTime;
        author.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, TargetObjectId, "test_viral_ack_building", "test_viral_ack_material", now);
        author.SocialPosts[0].Help(firstHelper, now);
        author.SocialPosts[0].Help(secondHelper, now);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return (author, firstHelper, secondHelper);
    }

    private async Task<List<SocialPostHelper>> GetUnseenHelpers()
    {
        return await Context.Set<SocialPostHelper>()
            .AsNoTracking()
            .Include(x => x.Helper)
            .Where(x => !x.AckSeen)
            .ToListAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task Seen_WithoutHelper_MarksEveryHelperOfTheBuilding()
    {
        var (author, _, _) = await SeedHelpedBuildingPost();

        var response = await new Seen(Context).HandlePacket(new SeenRequest { TargetObjectId = TargetObjectId, HelperZid = "" }, author.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (await GetUnseenHelpers()).Should().BeEmpty();
    }

    [Fact]
    public async Task Seen_WithHelper_MarksOnlyThatHelper()
    {
        var (author, firstHelper, secondHelper) = await SeedHelpedBuildingPost();

        await new Seen(Context).HandlePacket(new SeenRequest { TargetObjectId = TargetObjectId, HelperZid = firstHelper.Snuid.ToString() }, author.Id, TestContext.Current.CancellationToken);

        (await GetUnseenHelpers()).Should().ContainSingle().Which.Helper!.Id.Should().Be(secondHelper.Id);
    }

    [Fact]
    public async Task Seen_OtherBuilding_KeepsHelpersUnseen()
    {
        var (author, _, _) = await SeedHelpedBuildingPost();

        var response = await new Seen(Context).HandlePacket(new SeenRequest { TargetObjectId = TargetObjectId + 1, HelperZid = "" }, author.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (await GetUnseenHelpers()).Should().HaveCount(2);
    }
}
