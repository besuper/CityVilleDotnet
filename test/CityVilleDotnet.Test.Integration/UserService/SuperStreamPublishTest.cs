using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
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
}
