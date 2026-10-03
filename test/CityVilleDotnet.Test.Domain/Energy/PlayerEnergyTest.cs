using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Energy;

[Collection("Domain")]
public class PlayerEnergyTest
{
    [Fact]
    public void Player_RemoveEnergy_EnoughEnergy_DecreasesEnergy()
    {
        var faker = new Faker();
        var player = faker.Player();
        var energyBefore = player.Energy;

        player.RemoveEnergy(3);

        player.Energy.Should().Be(energyBefore - 3);
    }

    [Fact]
    public void Player_RemoveEnergy_NotEnoughEnergy_ThrowsForceReload()
    {
        var faker = new Faker();
        var player = faker.Player();
        player.SetEnergy(0);

        var act = () => player.RemoveEnergy(1);

        act.Should().Throw<DomainException>().Which.Reason.Should().Be(GameErrorType.ForceReload);
    }

    [Fact]
    public void Player_AddEnergy_AboveMax_IsNotCapped()
    {
        var faker = new Faker();
        var player = faker.Player();
        var energyMax = player.GetEnergyMax();
        player.SetEnergy(energyMax);

        player.AddEnergy(10);

        player.Energy.Should().Be(energyMax + 10);
    }

    [Fact]
    public void Player_RemoveEnergy_AboveMax_KeepsOverflow()
    {
        var faker = new Faker();
        var player = faker.Player();
        var energyMax = player.GetEnergyMax();
        player.SetEnergy(energyMax);
        player.AddEnergy(10);

        player.RemoveEnergy(1);

        player.Energy.Should().Be(energyMax + 9);
    }

    [Fact]
    public void Player_UpdateEnergy_AboveMax_KeepsOverflow()
    {
        var faker = new Faker();
        var player = faker.Player();
        var energyMax = player.GetEnergyMax();
        player.SetEnergy(energyMax);
        player.AddEnergy(10);

        player.UpdateEnergy();

        player.Energy.Should().Be(energyMax + 10);
    }
}
