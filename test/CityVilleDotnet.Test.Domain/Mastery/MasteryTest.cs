using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Domain.Fixtures;

namespace CityVilleDotnet.Test.Domain.Mastery;

[Collection("Domain")]
public class MasteryTest(DomainFixture fixture)
{
    [Fact]
    public void Player_IncrementMastery_CreatesNewMastery()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.IncrementMastery("plot_strawberries");

        player.Masteries.Count.Should().Be(1);
        player.Masteries.First().ItemName.Should().Be("plot_strawberries");
    }

    [Fact]
    public void Player_IncrementMastery_IncrementsCount()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.IncrementMastery("plot_strawberries");
        player.IncrementMastery("plot_strawberries");
        player.IncrementMastery("plot_strawberries");

        player.Masteries.First().Count.Should().Be(3);
    }

    [Fact]
    public void Player_IncrementMastery_DoesNotCreateDuplicate()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.IncrementMastery("plot_strawberries");
        player.IncrementMastery("plot_strawberries");

        player.Masteries.Count.Should().Be(1);
    }

    [Fact]
    public void Player_IncrementMastery_DifferentItems_CreatesSeparateMasteries()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.IncrementMastery("plot_strawberries");
        player.IncrementMastery("plot_corn");

        player.Masteries.Count.Should().Be(2);
    }

    [Fact]
    public void Player_IncrementMastery_StartsAtLevelZero()
    {
        var faker = new Faker();
        var player = faker.Player();

        player.IncrementMastery("plot_strawberries");

        player.Masteries.First().Level.Should().Be(0);
    }

    [Fact]
    public void Player_IncrementMastery_Strawberries_NoLevelUpBeforeReq()
    {
        var faker = new Faker();
        var player = faker.Player();

        for (var i = 0; i < 49; i++) player.IncrementMastery("plot_strawberries");

        player.Masteries.First().Level.Should().Be(0);
        player.Masteries.First().Count.Should().Be(49);
    }

    [Fact]
    public void Player_IncrementMastery_Strawberries_LevelsUpAtFifty()
    {
        var faker = new Faker();
        var player = faker.Player();

        for (var i = 0; i < 50; i++) player.IncrementMastery("plot_strawberries");

        player.Masteries.First().Level.Should().Be(1);
        player.Masteries.First().Count.Should().Be(50);
    }

    [Fact]
    public void Player_IncrementMastery_Strawberries_LevelsUpToTwoAtOneFifty()
    {
        var faker = new Faker();
        var player = faker.Player();

        for (var i = 0; i < 150; i++) player.IncrementMastery("plot_strawberries");

        player.Masteries.First().Level.Should().Be(2);
        player.Masteries.First().Count.Should().Be(150);
    }

    [Fact]
    public void Player_IncrementMastery_Strawberries_LevelsUpToThreeAtThreeHundred()
    {
        var faker = new Faker();
        var player = faker.Player();

        for (var i = 0; i < 300; i++) player.IncrementMastery("plot_strawberries");

        player.Masteries.First().Level.Should().Be(3);
        player.Masteries.First().Count.Should().Be(300);
    }

    [Fact]
    public void Player_IncrementMastery_Strawberries_ProgressesThroughAllLevels()
    {
        var faker = new Faker();
        var player = faker.Player();

        for (var i = 0; i < 50; i++) player.IncrementMastery("plot_strawberries");
        player.Masteries.First().Level.Should().Be(1);

        for (var i = 0; i < 100; i++) player.IncrementMastery("plot_strawberries");
        player.Masteries.First().Level.Should().Be(2);

        for (var i = 0; i < 150; i++) player.IncrementMastery("plot_strawberries");
        player.Masteries.First().Level.Should().Be(3);
    }

    [Fact]
    public void Player_IncrementMastery_Corn_LevelsUpAtSixty()
    {
        var faker = new Faker();
        var player = faker.Player();

        for (var i = 0; i < 60; i++) player.IncrementMastery("plot_corn");

        player.Masteries.First().Level.Should().Be(1);
        player.Masteries.First().Count.Should().Be(60);
    }

    [Fact]
    public void Player_IncrementMastery_Corn_LevelsUpToTwoAtOneEighty()
    {
        var faker = new Faker();
        var player = faker.Player();

        for (var i = 0; i < 180; i++) player.IncrementMastery("plot_corn");

        player.Masteries.First().Level.Should().Be(2);
        player.Masteries.First().Count.Should().Be(180);
    }

    [Fact]
    public void Player_IncrementMastery_Corn_StaysAtMaxLevelBeyondReq()
    {
        var faker = new Faker();
        var player = faker.Player();

        for (var i = 0; i < 300; i++) player.IncrementMastery("plot_corn");

        player.Masteries.First().Level.Should().Be(2);
        player.Masteries.First().Count.Should().Be(300);
    }

    [Fact]
    public void Player_IncrementMastery_CountIsPreservedAcrossLevelUps()
    {
        var faker = new Faker();
        var player = faker.Player();

        for (var i = 0; i < 150; i++) player.IncrementMastery("plot_strawberries");

        player.Masteries.First().Count.Should().Be(150);
        player.Masteries.First().Level.Should().Be(2);
    }

    [Fact]
    public void Player_IncrementMastery_UnknownItem_DoesNotCrash()
    {
        var faker = new Faker();
        var player = faker.Player();

        var act = () => player.IncrementMastery("unknown_item_that_does_not_exist");

        act.Should().NotThrow();
        player.Masteries.First().Count.Should().Be(1);
        player.Masteries.First().Level.Should().Be(0);
    }
}