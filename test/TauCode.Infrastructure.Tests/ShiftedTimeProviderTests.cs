using NUnit.Framework;
using TauCode.Infrastructure.Time;

namespace TauCode.Infrastructure.Tests;

[TestFixture]
public class ShiftedTimeProviderTests
{
    [Test]
    public async Task CreateTimeMachine_CertainTime_CreatesTimeMachine()
    {
        // Arrange
        await Task.Delay(100); // make NUnit infrastructure wake up

        var past = new DateTimeOffset(2010, 1, 1, 10, 20, 30, TimeSpan.Zero);
        var timeMachine = ShiftedTimeProvider.CreateTimeMachine(past);
        TimeProvider.Override(timeMachine);

        // Act
        var timeout = TimeSpan.FromMilliseconds(777);
        await Task.Delay(timeout);

        // Assert
        var fakeNow = TimeProvider.GetCurrentTime();
        var tolerance = TimeSpan.FromMilliseconds(20);
        Assert.That(fakeNow, Is.EqualTo(past.Add(timeout)).Within(tolerance));
    }
}