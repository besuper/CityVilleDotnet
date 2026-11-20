using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class StreamPublishTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private async Task<Player> SeedPlayer()
    {
        var player = Faker.Player();

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return player;
    }

    private async Task<SocialPost> GetSavedPost(Player author)
    {
        return await Context.Set<SocialPost>()
            .AsNoTracking()
            .Include(x => x.TargetPlayer)
            .SingleAsync(x => x.Player!.Id == author.Id, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task StreamPublish_OwnWall_SavesPostWithoutTarget()
    {
        var author = await SeedPlayer();
        var request = new StreamPublishRequest
        {
            Type = "test_viral_help",
            Data = new ASObject { ["level"] = 5 },
            TargetId = author.Snuid.ToString(),
            UserMessage = "hello"
        };

        var response = await new StreamPublish(Context).HandlePacket(request, author.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (response["data"] as ASObject)!["lastSuperFeedPostError"].Should().Be(0);
        var post = await GetSavedPost(author);
        post.Key.Should().Be("test_viral_help");
        post.Data.Should().Be("{\"level\":5}");
        post.UserMessage.Should().Be("hello");
        post.MaxHelpers.Should().Be(2);
        post.TargetPlayer.Should().BeNull();
    }

    [Fact]
    public async Task StreamPublish_FriendWall_SavesTargetPlayer()
    {
        var author = await SeedPlayer();
        var target = await SeedPlayer();
        var request = new StreamPublishRequest { Type = "test_viral_help", TargetId = target.Snuid.ToString() };

        await new StreamPublish(Context).HandlePacket(request, author.Id, TestContext.Current.CancellationToken);

        var post = await GetSavedPost(author);
        post.TargetPlayer!.Id.Should().Be(target.Id);
    }

    [Fact]
    public async Task StreamPublish_InvalidTarget_SavesPostWithoutTarget()
    {
        var author = await SeedPlayer();
        var request = new StreamPublishRequest { Type = "test_viral_help", TargetId = "not_a_snuid" };

        await new StreamPublish(Context).HandlePacket(request, author.Id, TestContext.Current.CancellationToken);

        var post = await GetSavedPost(author);
        post.TargetPlayer.Should().BeNull();
    }

    [Fact]
    public async Task StreamPublish_UnknownViral_SavesPostWithoutHelp()
    {
        var author = await SeedPlayer();
        var request = new StreamPublishRequest { Type = Faker.Random.String2(128) };

        await new StreamPublish(Context).HandlePacket(request, author.Id, TestContext.Current.CancellationToken);

        var post = await GetSavedPost(author);
        post.MaxHelpers.Should().Be(0);
    }
}
