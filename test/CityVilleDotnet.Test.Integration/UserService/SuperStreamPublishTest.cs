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
public class SuperStreamPublishTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task SuperStreamPublish_FriendWall_SavesPostWithTarget()
    {
        var author = Faker.Player();
        var target = Faker.Player();
        await Context.AddRangeAsync([author, target], TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var request = new SuperStreamPublishRequest
        {
            Type = "test_viral_help",
            Data = new ASObject { ["building"] = "Town Hall" },
            TargetId = target.Snuid.ToString(),
            UserMessage = "thanks"
        };

        var response = await new SuperStreamPublish(Context).HandlePacket(request, author.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        (response["data"] as ASObject)!["lastSuperFeedPostError"].Should().Be(0);
        var post = await Context.Set<SocialPost>()
            .AsNoTracking()
            .Include(x => x.TargetPlayer)
            .SingleAsync(x => x.Player!.Id == author.Id, TestContext.Current.CancellationToken);
        post.Key.Should().Be("test_viral_help");
        post.Data.Should().Be("{\"building\":\"Town Hall\"}");
        post.UserMessage.Should().Be("thanks");
        post.TargetPlayer!.Id.Should().Be(target.Id);
    }

    [Fact]
    public async Task SuperStreamPublish_ClickthroughParamsWithoutTarget_SavesPostWithoutTargetObject()
    {
        var author = Faker.Player();
        await Context.AddAsync(author, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        object[] parameters = ["test_viral_help", new ASObject { ["hotelName"] = "test_hotel" }, author.Snuid.ToString(), new ASObject { ["hotelId"] = 16777345, ["guestID"] = null }, null!, null!];

        var response = await new SuperStreamPublish(Context).HandlePacket(parameters, author.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var post = await Context.Set<SocialPost>().AsNoTracking().SingleAsync(x => x.Player!.Id == author.Id, TestContext.Current.CancellationToken);
        post.TargetObjectId.Should().BeNull();
    }

    // qm_test_hotels: countStreamPublishByName on test_viral_checkin,test_viral_checkin_2
    [Fact]
    public async Task SuperStreamPublish_QuestViral_ProgressesCountStreamPublishByName()
    {
        var author = Faker.Player();
        author.Quests.Add(Faker.Quest(name: "qm_test_hotels", length: 3));
        await Context.AddAsync(author, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        await new SuperStreamPublish(Context).HandlePacket(new SuperStreamPublishRequest { Type = "test_viral_checkin_2" }, author.Id, TestContext.Current.CancellationToken);

        Context.ChangeTracker.Clear();
        var quest = await Context.Set<Quest>().SingleAsync(x => x.Name == "qm_test_hotels", TestContext.Current.CancellationToken);
        quest.Progress.Should().Equal(0, 0, 1);
    }
}
