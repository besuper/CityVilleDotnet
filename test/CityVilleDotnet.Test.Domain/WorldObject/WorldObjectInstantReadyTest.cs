using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.WorldObject;

// instantReadyResidenceCostConstant5 = 2.0, test_skyscraper growTime 1 (23h), test_corporation timerDuration 600
[Collection("Domain")]
public class WorldObjectInstantReadyTest
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void WorldObject_GetUnroundedCostToMakeReady_Skyscraper_UsesResidenceFormula()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var skyscraper = faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        skyscraper.GetUnroundedCostToMakeReady().Should().BeApproximately(2.0 * Math.Pow(23, 0.25), 0.0001);
        skyscraper.GetCostToMakeReady().Should().Be(5);
    }

    [Fact]
    public void WorldObject_GetUnroundedCostToMakeReady_Corporation_UsesHarvestEndTime()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var corporation = faker.WorldObject(itemName: "test_corporation", className: BuildingClassType.Corporation, state: WorldObjectState.Planted);
        corporation.InitHarvestEndTime();

        corporation.GetUnroundedCostToMakeReady().Should().BeApproximately(2.0 * Math.Pow(600 / 3600.0, 0.25), 0.0001);
        corporation.GetCostToMakeReady().Should().Be(2);
    }

    [Fact]
    public void WorldObject_GetUnroundedCostToMakeReady_CorporationTimerFinished_ReturnsZero()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var corporation = faker.WorldObject(itemName: "test_corporation", className: BuildingClassType.Corporation, state: WorldObjectState.Planted);
        corporation.InitHarvestEndTime();
        time.Advance(TimeSpan.FromSeconds(700));

        corporation.GetUnroundedCostToMakeReady().Should().Be(0);
        corporation.GetCostToMakeReady().Should().Be(1);
    }

    [Fact]
    public void WorldObject_GetUnroundedCostToMakeReady_LaunchPad_ReturnsFixedCost()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var launchPad = faker.WorldObject(itemName: "test_launchpad", className: BuildingClassType.LaunchPad, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        launchPad.GetCostToMakeReady().Should().Be(3);
    }

    [Fact]
    public void WorldObject_MakeInstantReady_Corporation_ResetsHarvestEndTimeAndKeepsState()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var corporation = faker.WorldObject(itemName: "test_corporation", className: BuildingClassType.Corporation, state: WorldObjectState.Planted);
        corporation.InitHarvestEndTime();

        corporation.MakeInstantReady();

        corporation.HarvestEndTime.Should().Be(StartTime.UtcDateTime);
        corporation.State.Should().Be(WorldObjectState.Planted);
    }

    [Fact]
    public void WorldObject_MakeInstantReady_Skyscraper_ResetsHarvestEndTimeAndKeepsState()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var skyscraper = faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());
        skyscraper.InitHarvestEndTime();

        skyscraper.MakeInstantReady();

        skyscraper.HarvestEndTime.Should().Be(StartTime.UtcDateTime);
        skyscraper.State.Should().Be(WorldObjectState.Planted);
    }

    [Fact]
    public void WorldObject_MakeInstantReady_LaunchPad_SetsGrown()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var launchPad = faker.WorldObject(itemName: "test_launchpad", className: BuildingClassType.LaunchPad, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime());

        launchPad.MakeInstantReady();

        launchPad.State.Should().Be(WorldObjectState.Grown);
        launchPad.HarvestEndTime.Should().BeNull();
    }
}
