using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.Energy;

[Collection("Domain")]
public class PlayerEnergyRegenerationTest(DomainFixture fixture)
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);
    private static TimeSpan RegenCycle => TimeSpan.FromSeconds(GameSettingsManager.Instance.GetSettings().EnergyRegenerationSeconds);

    [Fact]
    public void Player_UpdateEnergy_AfterTwoRegenCycles_RecoversTwoEnergy()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var player = faker.Player();
        player.RemoveEnergy(5);

        time.Advance(RegenCycle * 2);
        player.UpdateEnergy();

        player.Energy.Should().Be(9);
    }

    [Fact]
    public void Player_UpdateEnergy_PartialCycle_KeepsRegenProgress()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var player = faker.Player();
        player.RemoveEnergy(5);

        time.Advance(RegenCycle * 1.5);
        player.UpdateEnergy();

        player.Energy.Should().Be(8);

        time.Advance(RegenCycle * 0.5);
        player.UpdateEnergy();

        player.Energy.Should().Be(9);
    }

    [Fact]
    public void Player_UpdateEnergy_LongAbsence_CapsAtMax()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var player = faker.Player();
        player.RemoveEnergy(5);

        time.Advance(RegenCycle * 100);
        player.UpdateEnergy();

        player.Energy.Should().Be(player.GetEnergyMax());
    }

    [Fact]
    public void Player_RemoveEnergy_FromMax_RestartsRegenTimer()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var player = faker.Player();

        time.Advance(RegenCycle * 10);
        player.RemoveEnergy(1);

        time.Advance(RegenCycle - TimeSpan.FromSeconds(1));
        player.UpdateEnergy();

        player.Energy.Should().Be(11);

        time.Advance(TimeSpan.FromSeconds(1));
        player.UpdateEnergy();

        player.Energy.Should().Be(12);
    }

    [Fact]
    public void Player_RemoveEnergy_BelowMax_KeepsRegenProgress()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var player = faker.Player();
        player.RemoveEnergy(5);

        time.Advance(RegenCycle * 0.75);
        player.RemoveEnergy(1);

        time.Advance(RegenCycle * 0.25);
        player.UpdateEnergy();

        player.Energy.Should().Be(7);
    }
}
