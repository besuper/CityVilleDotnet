using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.SocialPost;

[Collection("Domain")]
public class PlayerSocialTest
{
    private static readonly DateTime Now = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Player_Publish_WithTarget_AddsPostLinkedToAuthorAndTarget()
    {
        var faker = new Faker();
        var player = faker.Player();
        var target = faker.Player();

        player.Publish(SocialPostType.Feed, "test_viral_help", "{\"level\":5}", null, target, 0, null, null, Now);

        var post = player.SocialPosts.Should().ContainSingle().Subject;
        post.Player.Should().Be(player);
        post.TargetPlayer.Should().Be(target);
        post.Key.Should().Be("test_viral_help");
        post.Data.Should().Be("{\"level\":5}");
        post.CreatedAt.Should().Be(Now);
    }

    [Fact]
    public void Player_GrantViralRewards_UnsupportedType_IsSkipped()
    {
        var faker = new Faker();
        var player = faker.Player();
        var gold = player.Gold;

        var granted = player.GrantViralRewards([
            new ViralReward { Type = "reward_coins", Data = new ViralRewardData { Amount = 10 } },
            new ViralReward { Type = "reward_loot", Data = new ViralRewardData { Amount = 1 } }
        ]);

        player.Gold.Should().Be(gold + 10);
        granted.Should().ContainSingle().Which.Type.Should().Be("reward_coins");
    }

    [Fact]
    public void Player_GetActiveVirals_PostsInsideReset_ReturnsCountAndOrigin()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, 0, null, null, Now.AddHours(-3));
        player.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, 0, null, null, Now.AddHours(-1));

        var activeVirals = player.GetActiveVirals(Now);

        activeVirals.Should().ContainSingle().Which.Should().Be(("test_viral_help", 2, Now.AddHours(-3)));
    }

    [Fact]
    public void Player_GetActiveVirals_PostsAfterReset_AreIgnored()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, 0, null, null, Now.AddHours(-5));
        player.Publish(SocialPostType.Feed, "test_viral_no_reward", "{}", null, null, 0, null, null, Now.AddHours(-3));

        var activeVirals = player.GetActiveVirals(Now);

        activeVirals.Should().BeEmpty();
    }

    [Fact]
    public void Player_GetViralAcks_UnseenHelpOnTarget_ReturnsHelper()
    {
        var faker = new Faker();
        var (author, helper) = PublishTargetedPostHelpedBy(faker, 101);

        var acks = author.GetViralAcks(author.LastPlayedWorldType, Now.AddHours(2));

        acks.Should().ContainSingle().Which.Should().Be((42, "test_viral_ack_building", "test_viral_ack_material", helper.Snuid, Now.AddHours(1)));
    }

    [Fact]
    public void Player_GetViralAcks_PostWithoutTarget_IsIgnored()
    {
        var faker = new Faker();
        var author = faker.Player();
        author.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, 0, null, null, Now);
        author.SocialPosts[0].Help(faker.Player(), Now.AddHours(1));

        var acks = author.GetViralAcks(author.LastPlayedWorldType, Now.AddHours(2));

        acks.Should().BeEmpty();
    }

    [Fact]
    public void Player_GetViralAcks_HelpOlderThanLifetime_IsIgnored()
    {
        var faker = new Faker();
        var (author, _) = PublishTargetedPostHelpedBy(faker, 101);

        var acks = author.GetViralAcks(author.LastPlayedWorldType, Now.AddHours(1).Add(Player.ViralAckLifetime));

        acks.Should().BeEmpty();
    }

    [Fact]
    public void Player_GetViralAcks_OtherWorld_IsIgnored()
    {
        var faker = new Faker();
        var (author, _) = PublishTargetedPostHelpedBy(faker, 101);

        var acks = author.GetViralAcks(WorldType.Downtown, Now.AddHours(2));

        acks.Should().BeEmpty();
    }

    [Fact]
    public void Player_MarkViralAckSeen_WithoutHelper_MarksEveryHelperOfTheTarget()
    {
        var faker = new Faker();
        var (author, _) = PublishTargetedPostHelpedBy(faker, 101);
        author.SocialPosts[0].Help(PlayerWithSnuid(faker, 102), Now.AddHours(1));

        author.MarkViralAckSeen(author.LastPlayedWorldType, 42, null);

        author.SocialPosts[0].Helpers.Should().OnlyContain(x => x.AckSeen);
        author.GetViralAcks(author.LastPlayedWorldType, Now.AddHours(2)).Should().BeEmpty();
    }

    [Fact]
    public void Player_MarkViralAckSeen_WithHelper_MarksOnlyThatHelper()
    {
        var faker = new Faker();
        var (author, _) = PublishTargetedPostHelpedBy(faker, 101);
        var otherHelper = PlayerWithSnuid(faker, 102);
        author.SocialPosts[0].Help(otherHelper, Now.AddHours(1));

        author.MarkViralAckSeen(author.LastPlayedWorldType, 42, 101);

        author.GetViralAcks(author.LastPlayedWorldType, Now.AddHours(2)).Should().ContainSingle().Which.HelperSnuid.Should().Be(otherHelper.Snuid);
    }

    [Fact]
    public void Player_MarkViralAckSeen_OtherTarget_IsUntouched()
    {
        var faker = new Faker();
        var (author, _) = PublishTargetedPostHelpedBy(faker, 101);

        author.MarkViralAckSeen(author.LastPlayedWorldType, 43, null);

        author.SocialPosts[0].Helpers.Should().OnlyContain(x => !x.AckSeen);
    }

    private static Player PlayerWithSnuid(Faker faker, int snuid)
    {
        var player = faker.Player();
        player.Snuid = snuid;

        return player;
    }

    private static (Player Author, Player Helper) PublishTargetedPostHelpedBy(Faker faker, int helperSnuid)
    {
        var author = PlayerWithSnuid(faker, 100);
        var helper = PlayerWithSnuid(faker, helperSnuid);
        author.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, 42, "test_viral_ack_building", "test_viral_ack_material", Now);
        author.SocialPosts[0].Help(helper, Now.AddHours(1));

        return (author, helper);
    }
}
