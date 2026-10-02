using AwesomeAssertions;
using CityVilleDotnet.Common.Utils;
using Microsoft.Extensions.Time.Testing;

namespace CityVilleDotnet.Test.Domain.Utils;

public class ServerUtilsTest
{
    private static readonly DateTimeOffset StartTime = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void ServerUtils_GetActionTime_NoClientTime_ReturnsNow()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);

        ServerUtils.GetActionTime(null).Should().Be(StartTime.ToUnixTimeMilliseconds());
        ServerUtils.GetActionTime(0).Should().Be(StartTime.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void ServerUtils_GetActionTime_RecentClientTime_ReturnsClientTimeInMs()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var clientTime = StartTime.AddSeconds(-30).ToUnixTimeSeconds();

        ServerUtils.GetActionTime(clientTime).Should().Be(clientTime * 1000);
    }

    [Fact]
    public void ServerUtils_GetActionTime_ClientTimeTooOld_ClampsToOneMinuteAgo()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var clientTime = StartTime.AddMinutes(-5).ToUnixTimeSeconds();

        ServerUtils.GetActionTime(clientTime).Should().Be(StartTime.AddMinutes(-1).ToUnixTimeMilliseconds());
    }

    [Fact]
    public void ServerUtils_GetActionTime_ClientTimeInFuture_ClampsToNow()
    {
        ServerUtils.TimeProvider = new FakeTimeProvider(StartTime);
        var clientTime = StartTime.AddSeconds(30).ToUnixTimeSeconds();

        ServerUtils.GetActionTime(clientTime).Should().Be(StartTime.ToUnixTimeMilliseconds());
    }
}
