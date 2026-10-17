using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Factory.Flag;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.Flag;

public class FlagTest
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Flag_Constructor_SetsLastModifiedToNow()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var faker = new Faker();

        var flag = faker.Flag(name: "test_flag", value: 3);

        flag.Name.Should().Be("test_flag");
        flag.Value.Should().Be(3);
        flag.LastModified.Should().Be(StartTime);
    }

    [Fact]
    public void Flag_SetValue_UpdatesValueAndLastModified()
    {
        var time = new FakeTimeProvider(StartTime);
        ServerUtils.TimeProvider = time;
        var faker = new Faker();
        var flag = faker.Flag(value: 1);

        time.Advance(TimeSpan.FromHours(2));
        flag.SetValue(-2);

        flag.Value.Should().Be(-2);
        flag.LastModified.Should().Be(StartTime.AddHours(2));
    }
}
