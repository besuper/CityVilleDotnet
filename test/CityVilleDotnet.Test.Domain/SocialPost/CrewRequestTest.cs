using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using WorldObjectEntity = CityVilleDotnet.Domain.Entities.WorldObject;

namespace CityVilleDotnet.Test.Domain.SocialPost;

// test_crew_upgrade: the first gate ("build") needs 4 crew members
[Collection("Domain")]
public class CrewRequestTest
{
    private static readonly DateTime Now = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    private static (Player Author, WorldObjectEntity Building) AuthorWithCrewBuilding(Faker faker)
    {
        var building = faker.WorldObject(itemName: "test_crew_upgrade");
        var author = faker.Player(world: faker.World(objects: [building]));

        return (author, building);
    }

    private static Player FriendOf(Faker faker, Player author)
    {
        var friend = faker.Player();
        author.SendFriendRequest(friend);

        foreach (var friendship in author.Friends.Concat(friend.Friends))
        {
            friendship.Status = FriendshipStatus.Accepted;
        }

        return friend;
    }

    [Fact]
    public void Player_RequestCrew_FreeSlots_CreatesCrewPostOnBuilding()
    {
        var faker = new Faker();
        var (author, building) = AuthorWithCrewBuilding(faker);
        building.AddCrewMember(null);

        author.RequestCrew(building, null, Now);

        var post = author.SocialPosts.Should().ContainSingle().Subject;
        post.Type.Should().Be(SocialPostType.Crew);
        post.Key.Should().Be("test_crew_upgrade");
        post.TargetObjectId.Should().Be(building.WorldFlatId);
        post.TargetObjectName.Should().Be("test_crew_upgrade");
        post.TargetWorld.Should().Be(author.LastPlayedWorldType);
        post.MaxHelpers.Should().Be(3);
        post.ExpiresAt.Should().Be(Now.AddDays(7));
    }

    [Fact]
    public void Player_RequestCrew_OpenRequestForBuilding_DoesNotDuplicate()
    {
        var faker = new Faker();
        var (author, building) = AuthorWithCrewBuilding(faker);
        author.RequestCrew(building, null, Now);

        author.RequestCrew(building, null, Now.AddHours(1));

        author.SocialPosts.Should().ContainSingle();
    }

    [Fact]
    public void Player_RequestCrew_CrewComplete_CreatesNothing()
    {
        var faker = new Faker();
        var (author, building) = AuthorWithCrewBuilding(faker);

        for (var i = 0; i < 4; i++)
        {
            building.AddCrewMember(null);
        }

        author.RequestCrew(building, null, Now);

        author.SocialPosts.Should().BeEmpty();
    }

    [Fact]
    public void SocialPost_Help_CrewByFriend_AddsFriendToBuildingCrew()
    {
        var faker = new Faker();
        var (author, building) = AuthorWithCrewBuilding(faker);
        var friend = FriendOf(faker, author);
        author.RequestCrew(building, null, Now);

        var rewards = author.SocialPosts[0].Help(friend, Now.AddHours(1));

        rewards.Should().BeEmpty();
        building.CrewMembers.Should().ContainSingle().Which.Player.Should().Be(friend);
        author.SocialPosts[0].Helpers.Should().ContainSingle().Which.Helper.Should().Be(friend);
    }

    [Fact]
    public void SocialPost_Help_CrewByNonFriend_ThrowsInvalidState()
    {
        var faker = new Faker();
        var (author, building) = AuthorWithCrewBuilding(faker);
        author.RequestCrew(building, null, Now);

        var act = () => author.SocialPosts[0].Help(faker.Player(), Now.AddHours(1));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
        building.CrewMembers.Should().BeEmpty();
        author.SocialPosts[0].Helpers.Should().BeEmpty();
    }

    [Fact]
    public void SocialPost_Help_CrewBuildingSold_ThrowsInvalidState()
    {
        var faker = new Faker();
        var author = faker.Player();
        var friend = FriendOf(faker, author);
        author.RequestCrew(faker.WorldObject(itemName: "test_crew_upgrade"), null, Now);

        var act = () => author.SocialPosts[0].Help(friend, Now.AddHours(1));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
        author.SocialPosts[0].Helpers.Should().BeEmpty();
    }

    [Fact]
    public void Player_GetViralAcks_CrewHelp_IsFlaggedAsCrew()
    {
        var faker = new Faker();
        var (author, building) = AuthorWithCrewBuilding(faker);
        var friend = FriendOf(faker, author);
        author.RequestCrew(building, null, Now);
        author.SocialPosts[0].Help(friend, Now.AddHours(1));

        var acks = author.GetViralAcks(author.LastPlayedWorldType, Now.AddHours(2));

        var ack = acks.Should().ContainSingle().Subject;
        ack.IsCrew.Should().BeTrue();
        ack.ItemName.Should().BeNull();
        ack.TargetObjectId.Should().Be(building.WorldFlatId);
    }
}
