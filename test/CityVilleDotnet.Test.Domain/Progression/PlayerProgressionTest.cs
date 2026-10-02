using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Progression;

[Collection("Domain")]
public class PlayerProgressionTest(DomainFixture fixture)
{
    [Fact]
    public void Player_AddXp_BelowNextLevel_StaysAtLevel()
    {
        var faker = new Faker();
        var player = faker.Player();
        var cashBefore = player.Cash;

        player.AddXp(3);

        player.Level.Should().Be(1);
        player.Xp.Should().Be(3);
        player.Cash.Should().Be(cashBefore);
    }

    [Fact]
    public void Player_AddXp_ReachesNextLevel_LevelsUpAndRefillsEnergy()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddXp(1);
        player.SetEnergy(3);
        var cashBefore = player.Cash;

        player.AddXp(3);

        player.Level.Should().Be(2);
        player.EnergyMax.Should().Be(13);
        player.Energy.Should().Be(13);
        player.Cash.Should().Be(cashBefore + 1);
    }

    [Fact]
    public void Player_AddXp_EnergyAboveNewMax_KeepsOverflow()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddXp(1);
        player.SetEnergy(20);

        player.AddXp(3);

        player.Level.Should().Be(2);
        player.Energy.Should().Be(20);
    }

    [Fact]
    public void Player_AddXp_SkipsSeveralLevels_JumpsToHighestReachedLevel()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.AddXp(1);
        var cashBefore = player.Cash;

        player.AddXp(30);

        player.Level.Should().Be(4);
        player.EnergyMax.Should().Be(15);
        player.Energy.Should().Be(15);
        player.Cash.Should().Be(cashBefore + 3);
    }
}
