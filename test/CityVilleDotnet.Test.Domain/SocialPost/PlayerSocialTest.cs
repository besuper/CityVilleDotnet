using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings.GameSettings;
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

        player.Publish(SocialPostType.Feed, "test_viral_help", "{\"level\":5}", null, target, Now);

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
        player.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, Now.AddHours(-3));
        player.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, Now.AddHours(-1));

        var activeVirals = player.GetActiveVirals(Now);

        activeVirals.Should().ContainSingle().Which.Should().Be(("test_viral_help", 2, Now.AddHours(-3)));
    }

    [Fact]
    public void Player_GetActiveVirals_PostsAfterReset_AreIgnored()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.Publish(SocialPostType.Feed, "test_viral_help", "{}", null, null, Now.AddHours(-5));
        player.Publish(SocialPostType.Feed, "test_viral_no_reward", "{}", null, null, Now.AddHours(-3));

        var activeVirals = player.GetActiveVirals(Now);

        activeVirals.Should().BeEmpty();
    }
}
