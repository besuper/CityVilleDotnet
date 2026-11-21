using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.SocialPost;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.SocialPost;

[Collection("Domain")]
public class SocialPostTest
{
    private static readonly DateTime Now = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void SocialPost_Create_KnownViral_UsesRewardSettings()
    {
        var faker = new Faker();

        var post = faker.SocialPost(key: "test_viral_help", now: Now);

        post.ExpiresAt.Should().Be(Now.AddHours(24));
        post.MaxHelpers.Should().Be(2);
    }

    [Fact]
    public void SocialPost_Create_ViralWithoutReward_CannotBeHelped()
    {
        var faker = new Faker();

        var post = faker.SocialPost(key: "test_viral_no_reward", now: Now);

        post.ExpiresAt.Should().Be(Now.AddHours(48));
        post.MaxHelpers.Should().Be(0);
        post.CanHelp(faker.Player().Id, Now).Should().BeFalse();
    }

    [Fact]
    public void SocialPost_Create_UnknownViral_KeepsPostWithoutHelp()
    {
        var faker = new Faker();

        var post = faker.SocialPost(now: Now);

        post.ExpiresAt.Should().Be(Now.AddHours(48));
        post.MaxHelpers.Should().Be(0);
    }

    [Fact]
    public void SocialPost_Create_RewardWithoutValues_UsesDefaults()
    {
        var faker = new Faker();

        var post = faker.SocialPost(key: "test_viral_empty_reward", now: Now);

        post.ExpiresAt.Should().Be(Now.AddHours(48));
        post.MaxHelpers.Should().Be(0);
    }

    [Fact]
    public void SocialPost_Create_LongUserMessage_IsTruncated()
    {
        var faker = new Faker();

        var post = faker.SocialPost(userMessage: faker.Random.String2(300));

        post.UserMessage.Should().HaveLength(256);
    }

    [Fact]
    public void SocialPost_Help_ValidHelper_GrantsHelperAndHostRewards()
    {
        var faker = new Faker();
        var author = faker.Player();
        var helper = faker.Player();
        var helperGold = helper.Gold;
        var post = faker.SocialPost(author: author, key: "test_viral_help", now: Now);

        var granted = post.Help(helper, Now);

        helper.Gold.Should().Be(helperGold + 50);
        author.CountInventoryItem("test_reward_item").Should().Be(1);
        granted.Should().ContainSingle().Which.Type.Should().Be("reward_coins");
        post.Helpers.Should().ContainSingle().Which.Helper.Should().Be(helper);
    }

    [Fact]
    public void SocialPost_Help_AfterMaxHostRewards_OnlyRewardsHelper()
    {
        var faker = new Faker();
        var author = faker.Player();
        var post = faker.SocialPost(author: author, key: "test_viral_help", now: Now);

        post.Help(faker.Player(), Now);
        post.Help(faker.Player(), Now);

        author.CountInventoryItem("test_reward_item").Should().Be(1);
        post.Helpers.Should().HaveCount(2);
    }

    [Fact]
    public void SocialPost_Help_OwnPost_ThrowsInvalidState()
    {
        var faker = new Faker();
        var author = faker.Player();
        var post = faker.SocialPost(author: author, key: "test_viral_help", now: Now);

        var act = () => post.Help(author, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void SocialPost_Help_Expired_ThrowsInvalidState()
    {
        var faker = new Faker();
        var post = faker.SocialPost(key: "test_viral_help", now: Now);

        var act = () => post.Help(faker.Player(), Now.AddHours(24));

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void SocialPost_Help_Full_ThrowsInvalidState()
    {
        var faker = new Faker();
        var post = faker.SocialPost(key: "test_viral_help", now: Now);
        post.Help(faker.Player(), Now);
        post.Help(faker.Player(), Now);

        var act = () => post.Help(faker.Player(), Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void SocialPost_Help_AlreadyHelped_ThrowsInvalidState()
    {
        var faker = new Faker();
        var helper = faker.Player();
        var post = faker.SocialPost(key: "test_viral_help", now: Now);
        post.Help(helper, Now);
        var helperGold = helper.Gold;

        var act = () => post.Help(helper, Now);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.InvalidState);
        helper.Gold.Should().Be(helperGold);
        post.Helpers.Should().ContainSingle();
    }

    [Fact]
    public void SocialPost_Create_WithTargetObject_SavesTargetAndAuthorWorld()
    {
        var faker = new Faker();
        var author = faker.Player();

        var post = faker.SocialPost(author: author, targetObjectId: 42, targetObjectName: "test_viral_ack_building", targetItemName: "test_viral_ack_material", now: Now);

        post.TargetObjectId.Should().Be(42);
        post.TargetObjectName.Should().Be("test_viral_ack_building");
        post.TargetItemName.Should().Be("test_viral_ack_material");
        post.TargetWorld.Should().Be(author.LastPlayedWorldType);
    }

    [Fact]
    public void SocialPost_Create_WithoutTargetObject_HasNoTarget()
    {
        var faker = new Faker();

        var post = faker.SocialPost(targetObjectId: 0, targetObjectName: "test_viral_ack_building", targetItemName: "test_viral_ack_material", now: Now);

        post.TargetObjectId.Should().BeNull();
        post.TargetObjectName.Should().BeNull();
        post.TargetItemName.Should().BeNull();
        post.TargetWorld.Should().BeNull();
    }
}
