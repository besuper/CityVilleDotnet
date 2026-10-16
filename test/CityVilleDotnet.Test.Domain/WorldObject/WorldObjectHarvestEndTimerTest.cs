using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Domain.Fixtures;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.WorldObject;

// test_skyscraper: growTime 1, test_corporation: harvestLoopConfig timerDuration 600
[Collection("Domain")]
public class WorldObjectHarvestEndTimerTest
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void WorldObject_HarvestEndTimer_NoTimer_SetsEndTimeFromDuration()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var skyscraper = faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper);

        skyscraper.HarvestEndTimer(3600, false);

        skyscraper.HarvestEndTime.Should().Be(StartTime.UtcDateTime.AddSeconds(3600));
    }

    [Fact]
    public void WorldObject_HarvestEndTimer_TimerNotFinished_ThrowsException()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var skyscraper = faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper);

        skyscraper.HarvestEndTimer(3600, false);
        time.Advance(TimeSpan.FromSeconds(3000));

        var act = () => skyscraper.HarvestEndTimer(3600, false);

        act.Should().Throw<Exception>();
        skyscraper.HarvestEndTime.Should().Be(StartTime.UtcDateTime.AddSeconds(3600));
    }

    [Fact]
    public void WorldObject_HarvestEndTimer_WithinOneMinuteTolerance_RestartsTimer()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var skyscraper = faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper);

        skyscraper.HarvestEndTimer(3600, false);
        time.Advance(TimeSpan.FromSeconds(3570));

        skyscraper.HarvestEndTimer(3600, false);

        skyscraper.HarvestEndTime.Should().Be(StartTime.UtcDateTime.AddSeconds(3570 + 3600));
    }

    [Fact]
    public void WorldObject_HarvestEndTimer_ZeroDuration_KeepsNoTimer()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var fishingHole = faker.WorldObject(itemName: "test_fishinghole", className: BuildingClassType.MechanicMapResource);

        fishingHole.HarvestEndTimer(0, false);

        fishingHole.HarvestEndTime.Should().BeNull();
    }

    [Fact]
    public void WorldObject_HarvestEndTimer_CountUpgradeAction_IncrementsUpgradeActionCount()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var skyscraper = faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper);

        skyscraper.HarvestEndTimer(0, true);
        skyscraper.HarvestEndTimer(0, true);

        skyscraper.UpgradeActionCount.Should().Be(2);
    }

    [Fact]
    public void WorldObject_InitHarvestEndTime_Skyscraper_UsesGrowTimeTimes23Hours()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var skyscraper = faker.WorldObject(itemName: "test_skyscraper", className: BuildingClassType.Skyscraper);

        skyscraper.InitHarvestEndTime();

        skyscraper.HarvestEndTime.Should().Be(StartTime.UtcDateTime.AddHours(23));
    }

    [Fact]
    public void WorldObject_InitHarvestEndTime_Corporation_UsesHarvestLoopTimer()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var corporation = faker.WorldObject(itemName: "test_corporation", className: BuildingClassType.Corporation);

        corporation.InitHarvestEndTime();

        corporation.HarvestEndTime.Should().Be(StartTime.UtcDateTime.AddSeconds(600));
    }

    [Fact]
    public void WorldObject_InitHarvestEndTime_Residence_KeepsNoTimer()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();
        var residence = faker.WorldObject(itemName: "test_res_base", className: BuildingClassType.Residence);

        residence.InitHarvestEndTime();

        residence.HarvestEndTime.Should().BeNull();
    }
}
