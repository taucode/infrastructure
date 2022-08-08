using NUnit.Framework;
using TauCode.Infrastructure.Time;

namespace TauCode.Infrastructure.Tests;

[TestFixture]
public class TimeProviderTests
{
    [SetUp]
    public void SetUp()
    {
        // idle
        TimeProvider.Reset();
    }

    [TearDown]
    public void TearDown()
    {
        TimeProvider.Reset();
    }

    [Test]
    public void TimeProvider_GetCurrent_ReturnsUtcNow()
    {
        // Arrange
        var utc = DateTimeOffset.UtcNow;

        // Act
        var timeProviderCurrent = TimeProvider.GetCurrentTime();

        // Assert
        Assert.That(timeProviderCurrent, Is.EqualTo(utc).Within(TimeSpan.FromMilliseconds(50)));
    }

    [Test]
    public void TimeProvider_OverrideWithTime_Overrides()
    {
        // Arrange
        var moment = new DateTimeOffset(2018, 12, 23, 1, 2, 3, TimeSpan.Zero);

        // Act
        TimeProvider.Override(moment);
        var current = TimeProvider.GetCurrentTime();

        // Assert
        Assert.That(current, Is.EqualTo(moment));
    }

    [Test]
    public void TimeProvider_OverrideWithTimeProvider_Overrides()
    {
        // Arrange
        var moment = new DateTimeOffset(2018, 12, 23, 1, 2, 3, TimeSpan.Zero);
        var provider = new ConstTimeProvider(moment);

        // Act
        TimeProvider.Override(provider);
        var current = TimeProvider.GetCurrentTime();

        // Assert
        Assert.That(current, Is.EqualTo(moment));
    }

    [Test]
    public void TimeProvider_Reset_Resets()
    {
        // Arrange
        var moment = new DateTimeOffset(2018, 12, 23, 1, 2, 3, TimeSpan.Zero);

        // Act
        TimeProvider.Override(moment);
        TimeProvider.Reset();
        var current = TimeProvider.GetCurrentTime();
        var utc = DateTimeOffset.UtcNow;

        // Assert
        Assert.That(current, Is.EqualTo(utc).Within(TimeSpan.FromMilliseconds(50)));
    }

    [Test]
    public async Task TimeProvider_ScaledTimeProvider_Scales()
    {
        // Arrange
        var moment = new DateTimeOffset(2018, 12, 23, 10, 0, 0, TimeSpan.Zero);

        var scale = 60; // one second becomes one minute

        var timeProvider1 = new ScaledTimeProvider(moment, +scale);
        var timeProvider2 = new ScaledTimeProvider(moment, -scale);

        var seconds = 10;
        var tolerance = 200;

        // Act
        await Task.Delay(seconds * 1000);

        TimeProvider.Override(timeProvider1);
        var time1 = TimeProvider.GetCurrentTime();

        TimeProvider.Override(timeProvider2);
        var time2 = TimeProvider.GetCurrentTime();

        // Assert
        // effect is +/- 10 minutes
        Assert.That(time1, Is.EqualTo(moment.AddMinutes(seconds)).Within(TimeSpan.FromMilliseconds(seconds * tolerance)));
        Assert.That(time2, Is.EqualTo(moment.AddMinutes(-seconds)).Within(TimeSpan.FromMilliseconds(seconds * tolerance)));
    }

    [Test]
    public void TimeProvider_ShiftedTimeProvider_Shifts()
    {
        // Arrange
        var shift = TimeSpan.FromHours(14.88);
        var shifted = new ShiftedTimeProvider(shift);

        // Act
        TimeProvider.Override(shifted);
        var time = TimeProvider.GetCurrentTime();
        var utc = DateTimeOffset.UtcNow;

        // Assert
        Assert.That(time, Is.EqualTo(utc.Add(shift)).Within(TimeSpan.FromMilliseconds(100)));
    }

}