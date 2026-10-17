using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.Flag;

[Collection("Domain")]
public class PlayerFlagTest
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Player_SetFlag_NewFlag_AddsFlag()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.SetFlag("test_flag", 4);

        player.Flags.Should().ContainSingle(x => x.Name == "test_flag" && x.Value == 4);
    }

    [Fact]
    public void Player_SetFlag_ExistingFlag_UpdatesValueWithoutDuplicate()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetFlag("test_flag", 1);

        player.SetFlag("test_flag", 2);

        player.Flags.Should().ContainSingle(x => x.Name == "test_flag").Which.Value.Should().Be(2);
    }

    [Fact]
    public void Player_CompleteTutorial_SetsCompletedTutorialFlag()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.CompleteTutorial();

        player.IsNew.Should().BeFalse();
        player.Flags.Should().ContainSingle(x => x.Name == "completed_tutorial" && x.Value == 1);
    }

    [Fact]
    public void Player_CompleteBridge_SetsCompletedBridgeFlag()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.CompleteBridge();

        player.Flags.Should().ContainSingle(x => x.Name == "completed_bridge" && x.Value == 1);
    }

    [Fact]
    public void Player_ToDto_NoFlag_SendsEmptyFlagContainer()
    {
        var faker = new Faker();
        var player = faker.Player();

        var dto = player.ToDto().UserInfo.Player;

        dto.FlagContainer.Should().BeEmpty();
    }

    [Fact]
    public void Player_ToDto_SendsStoredFlags()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var player = faker.Player();
        player.SetFlag("test_flag", -2);

        var dto = player.ToDto().UserInfo.Player;

        var flag = dto.FlagContainer.Should().ContainSingle().Subject;
        flag["name"].Should().Be("test_flag");
        flag["m_value"].Should().Be(-2);
        flag["lastModifiedGlobalEngineTime"].Should().Be(StartTime.ToUnixTimeMilliseconds());
    }
}
