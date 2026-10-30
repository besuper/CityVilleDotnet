using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.SeenFlag;

[Collection("Domain")]
public class PlayerSeenFlagTest
{
    [Fact]
    public void Player_SetSeenFlag_NewFlag_AddsBooleanFlag()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.SetSeenFlag("test_flag");

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_flag").Which.Value.Should().BeNull();
        player.HasSeenFlag("test_flag").Should().BeTrue();
    }

    [Fact]
    public void Player_SetSeenFlag_ExistingIntFlag_KeepsValue()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetSeenFlagInt("test_flag", 3);

        player.SetSeenFlag("test_flag");

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_flag").Which.Value.Should().Be(3);
    }

    [Fact]
    public void Player_SetSeenFlag_ExistingZeroFlag_BecomesTrue()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetSeenFlagInt("test_flag", 0);

        player.SetSeenFlag("test_flag");

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_flag").Which.Value.Should().BeNull();
    }

    [Fact]
    public void Player_SetSeenFlagInt_NewFlag_AddsValue()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.SetSeenFlagInt("test_flag", 5);

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_flag").Which.Value.Should().Be(5);
    }

    [Fact]
    public void Player_SetSeenFlagInt_ExistingFlag_OverwritesValue()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetSeenFlag("test_flag");

        player.SetSeenFlagInt("test_flag", 0);

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_flag").Which.Value.Should().Be(0);
    }

    [Fact]
    public void Player_IncrementSeenFlag_NewFlag_StartsAtOne()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.IncrementSeenFlag("test_flag");

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_flag").Which.Value.Should().Be(1);
    }

    [Fact]
    public void Player_IncrementSeenFlag_BooleanFlag_CountsAsOne()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetSeenFlag("test_flag");

        player.IncrementSeenFlag("test_flag");

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_flag").Which.Value.Should().Be(2);
    }

    [Fact]
    public void Player_IncrementSeenFlag_IntFlag_AddsOne()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetSeenFlagInt("test_flag", 0);

        player.IncrementSeenFlag("test_flag");
        player.IncrementSeenFlag("test_flag");

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_flag").Which.Value.Should().Be(2);
    }

    [Fact]
    public void Player_ToDto_SendsBooleanAndIntSeenFlags()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetSeenFlag("test_bool");
        player.SetSeenFlagInt("test_int", 4);
        player.SetSeenFlagInt("test_zero", 0);

        var seenFlags = player.ToDto().UserInfo.Player.SeenFlags;

        seenFlags["test_bool"].Should().Be(true);
        seenFlags["test_int"].Should().Be(4);
        seenFlags["test_zero"].Should().Be(0);
    }
}
