using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.SocialPost;

[Collection("Domain")]
public class ViralAckDtoTest
{
    private static readonly DateTimeOffset Now = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void ViralAckDto_ToViralAckDtos_GroupsHelpersByBuilding()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(Now);
        var faker = new Faker();
        var author = faker.Player();
        var helper = faker.Player();
        helper.Snuid = 101;
        var helpedAt = Now.UtcDateTime.AddHours(-1);
        author.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, 42, "test_viral_ack_building", "test_viral_ack_material", helpedAt);
        author.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, 0, null, null, helpedAt);
        author.SocialPosts[0].Help(helper, helpedAt);
        author.SocialPosts[1].Help(helper, helpedAt);

        var dtos = author.ToViralAckDtos();

        dtos.Should().HaveCount(1);
        var helperDto = dtos.Should().ContainKey("va42").WhoseValue.Helpers.Should().ContainSingle().Subject;
        helperDto.Zid.Should().Be("101");
        helperDto.ItemName.Should().Be("test_viral_ack_material");
        helperDto.TargetObjectName.Should().Be("test_viral_ack_building");
        helperDto.Ts.Should().Be(new DateTimeOffset(helpedAt).ToUnixTimeSeconds());
    }

    [Fact]
    public void ViralAckDto_ToViralAckDtos_CrewHelper_HasCrewPosAndNoItemName()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(Now);
        var faker = new Faker();
        var building = faker.WorldObject(itemName: "test_crew_upgrade");
        var author = faker.Player(world: faker.World(objects: [building]));
        var friend = faker.Player();
        author.SendFriendRequest(friend);

        foreach (var friendship in author.Friends.Concat(friend.Friends))
        {
            friendship.Status = FriendshipStatus.Accepted;
        }

        author.RequestCrew(building, null, Now.UtcDateTime.AddHours(-2));
        author.SocialPosts[0].Help(friend, Now.UtcDateTime.AddHours(-1));

        var helperDto = author.ToViralAckDtos().Should().ContainKey($"va{building.WorldFlatId}").WhoseValue.Helpers.Should().ContainSingle().Subject;

        helperDto.CrewPos.Should().Be(0);
        helperDto.ItemName.Should().BeNull();
    }
}
