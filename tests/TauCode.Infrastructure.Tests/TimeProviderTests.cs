using NUnit.Framework;
using System;
using System.Threading;
using TauCode.Infrastructure.Time;

namespace TauCode.Infrastructure.Tests
{
    [TestFixture]
    public class TimeProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            // idle
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
            var utcNow = DateTime.UtcNow;

            // Act
            var timeProviderCurrent = TimeProvider.GetCurrent();

            // Assert
            Assert.That(timeProviderCurrent, Is.EqualTo(utcNow).Within(TimeSpan.FromMilliseconds(50)));
        }

        [Test]
        public void TimeProvider_OverrideWithTime_Overrides()
        {
            // Arrange
            var dateTime = new DateTime(2018, 12, 23, 1, 2, 3);

            // Act
            TimeProvider.Override(dateTime);
            var current = TimeProvider.GetCurrent();

            // Assert
            Assert.That(current, Is.EqualTo(dateTime));
        }

        [Test]
        public void TimeProvider_OverrideWithTimeProvider_Overrides()
        {
            // Arrange
            var dateTime = new DateTime(2018, 12, 23, 1, 2, 3);
            var provider = new ConstTimeProvider(dateTime);

            // Act
            TimeProvider.Override(provider);
            var current = TimeProvider.GetCurrent();

            // Assert
            Assert.That(current, Is.EqualTo(dateTime));
        }

        [Test]
        public void TimeProvider_Reset_Resets()
        {
            // Arrange
            var dateTime = new DateTime(2018, 12, 23, 1, 2, 3);

            // Act
            TimeProvider.Override(dateTime);
            TimeProvider.Reset();
            var current = TimeProvider.GetCurrent();
            var utcNow = DateTime.UtcNow;

            // Assert
            Assert.That(current, Is.EqualTo(utcNow).Within(TimeSpan.FromMilliseconds(50)));
        }

        [Test]
        public void TimeProvider_ScaledTimeProvider_Scales()
        {
            // Arrange
            var dateTime = new DateTime(2018, 12, 23, 10, 0, 0);

            var scale = 60; // one second becomes one minute

            var timeProvider1 = new ScaledTimeProvider(dateTime, +scale);
            var timeProvider2 = new ScaledTimeProvider(dateTime, -scale);

            var seconds = 10;
            var tolerance = 100;

            // Act
            Thread.Sleep(seconds * 1000);

            TimeProvider.Override(timeProvider1);
            var time1 = TimeProvider.GetCurrent();

            TimeProvider.Override(timeProvider2);
            var time2 = TimeProvider.GetCurrent();

            // Assert
            // effect is +/- 10 minutes
            Assert.That(time1, Is.EqualTo(dateTime.AddMinutes(seconds)).Within(TimeSpan.FromMilliseconds(seconds * tolerance)));
            Assert.That(time2, Is.EqualTo(dateTime.AddMinutes(-seconds)).Within(TimeSpan.FromMilliseconds(seconds * tolerance)));
        }

        [Test]
        public void TimeProvider_ShiftedTimeProvider_Shifts()
        {
            // Arrange
            var shift = TimeSpan.FromHours(14.88);
            var shifted = new ShiftedTimeProvider(shift);

            // Act
            TimeProvider.Override(shifted);
            var time = TimeProvider.GetCurrent();
            var utcNow = DateTime.UtcNow;

            // Assert
            Assert.That(time, Is.EqualTo(utcNow.Add(shift)).Within(TimeSpan.FromMilliseconds(100)));
        }
    }
}
