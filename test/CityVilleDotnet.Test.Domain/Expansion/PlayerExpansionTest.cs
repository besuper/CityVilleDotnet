using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;

namespace CityVilleDotnet.Test.Domain.Expansion;

// TestData: population permits are 1,2,3 ; appraisal_downtown permits are 100 + num (61 rows) ; expansionDeltaMax = 10
[Collection("Domain")]
public class PlayerExpansionTest
{
    private static Player CreatePlayer(Faker faker, int expansionsPurchased, WorldType worldType = WorldType.Main, int lastExpansionTier = 0)
    {
        var player = faker.Player();

        if (worldType != WorldType.Main)
        {
            player.AddWorld(faker.World(type: worldType));
            player.SwitchWorld(worldType);
        }

        player.GetWorld().SetLastExpansionTier(lastExpansionTier);

        for (var i = 0; i < expansionsPurchased; i++)
        {
            player.IncrementExpansionsPurchased();
        }

        return player;
    }

    [Fact]
    public void Player_GetExpansionTier_MainWithoutLastTier_UsesNextExpansion()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 30);

        player.GetExpansionTier().Should().Be(31);
    }

    [Fact]
    public void Player_GetExpansionTier_MainWithLastTier_CapsByDeltaMax()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 30, lastExpansionTier: 5);

        player.GetExpansionTier().Should().Be(15);
    }

    [Fact]
    public void Player_GetExpansionTier_DowntownWithoutLastTier_CapsAtDeltaMax()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 30, worldType: WorldType.Downtown);

        player.GetExpansionTier().Should().Be(10);
    }

    [Fact]
    public void Player_GetExpansionTier_DowntownWithLastTier_CapsByDeltaMax()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 30, worldType: WorldType.Downtown, lastExpansionTier: 10);

        player.GetExpansionTier().Should().Be(20);
    }

    [Fact]
    public void Player_GetExpansionTier_LastTierAheadOfPurchases_UsesNextExpansion()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 3, worldType: WorldType.Downtown, lastExpansionTier: 3);

        player.GetExpansionTier().Should().Be(4);
    }

    [Fact]
    public void Player_GetRequiredExpansionPermits_Main_UsesPopulationRow()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 0);

        player.GetRequiredExpansionPermits(2).Should().Be(2);
    }

    [Fact]
    public void Player_GetRequiredExpansionPermits_Lakefront_UsesPopulationRow()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 0, worldType: WorldType.Lakefront);

        player.GetRequiredExpansionPermits(2).Should().Be(2);
    }

    [Fact]
    public void Player_GetRequiredExpansionPermits_TierAboveTable_UsesLastRow()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 0);

        player.GetRequiredExpansionPermits(140).Should().Be(3);
    }

    [Fact]
    public void Player_GetRequiredExpansionPermits_Downtown_UsesDowntownRow()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 0, worldType: WorldType.Downtown);

        player.GetRequiredExpansionPermits(5).Should().Be(105);
    }

    [Fact]
    public void Player_GetRequiredExpansionPermits_DowntownTierAboveSixty_UsesSixtiethRow()
    {
        var faker = new Faker();
        var player = CreatePlayer(faker, expansionsPurchased: 0, worldType: WorldType.Downtown);

        player.GetRequiredExpansionPermits(61).Should().Be(160);
    }
}
