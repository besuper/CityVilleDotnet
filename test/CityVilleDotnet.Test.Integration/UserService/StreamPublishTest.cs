using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
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

    [Fact]
    public async Task StreamPublish_BuildingMaterialFeed_SavesTargetObject()
    {
        var author = await SeedPlayer();
        var request = new StreamPublishRequest
        {
            Type = "test_viral_help",
            Data = new ASObject { ["material"] = "test_viral_ack_material" },
            FeedData = new StreamPublishFeedData { Material = "test_viral_ack_material" },
            ClickthroughParams = new StreamPublishClickthroughParams { TargetObjectId = 42, TargetObjectName = "test_viral_ack_building" }
        };

        await new StreamPublish(Context).HandlePacket(request, author.Id, TestContext.Current.CancellationToken);

        var post = await GetSavedPost(author);
        post.TargetObjectId.Should().Be(42);
        post.TargetObjectName.Should().Be("test_viral_ack_building");
        post.TargetItemName.Should().Be("test_viral_ack_material");
        post.TargetWorld.Should().Be(author.LastPlayedWorldType);
    }

    [Fact]
    public async Task StreamPublish_WithoutClickthroughParams_SavesPostWithoutTargetObject()
    {
        var author = await SeedPlayer();
        var request = new StreamPublishRequest { Type = "test_viral_help" };

        await new StreamPublish(Context).HandlePacket(request, author.Id, TestContext.Current.CancellationToken);

        var post = await GetSavedPost(author);
        post.TargetObjectId.Should().BeNull();
        post.TargetWorld.Should().BeNull();
    }

    // qm_test_hotels: countStreamPublishByName on test_viral_checkin,test_viral_checkin_2
    [Fact]
    public async Task StreamPublish_QuestViral_ProgressesCountStreamPublishByName()
    {
        var author = Faker.Player();
        author.Quests.Add(Faker.Quest(name: "qm_test_hotels", length: 3));
        await Context.AddAsync(author, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        await new StreamPublish(Context).HandlePacket(new StreamPublishRequest { Type = "test_viral_checkin" }, author.Id, TestContext.Current.CancellationToken);

        Context.ChangeTracker.Clear();
        var quest = await Context.Set<Quest>().SingleAsync(x => x.Name == "qm_test_hotels", TestContext.Current.CancellationToken);
        quest.Progress.Should().Equal(0, 0, 1);
    }

    [Fact]
    public async Task StreamPublish_OtherViral_KeepsQuestProgress()
    {
        var author = Faker.Player();
        author.Quests.Add(Faker.Quest(name: "qm_test_hotels", length: 3));
        await Context.AddAsync(author, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        await new StreamPublish(Context).HandlePacket(new StreamPublishRequest { Type = "test_viral_help" }, author.Id, TestContext.Current.CancellationToken);

        Context.ChangeTracker.Clear();
        var quest = await Context.Set<Quest>().SingleAsync(x => x.Name == "qm_test_hotels", TestContext.Current.CancellationToken);
        quest.Progress.Should().Equal(0, 0, 0);
    }
}
