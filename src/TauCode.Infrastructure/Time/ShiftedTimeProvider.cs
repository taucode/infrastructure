using System;

namespace TauCode.Infrastructure.Time
{
    public class ShiftedTimeProvider : TimeProviderBase
    {
        public ShiftedTimeProvider(TimeSpan shift)
        {
            this.Shift = shift;
        }

        public TimeSpan Shift { get; }

        public override DateTimeOffset GetCurrentTime() => DateTimeOffset.UtcNow + this.Shift;

// todo: this is confusing. we have TimeMachineTimeProvider.cs now.
        public static ShiftedTimeProvider CreateTimeMachine(DateTimeOffset fakeNow) =>
            new ShiftedTimeProvider(fakeNow - DateTimeOffset.UtcNow);
    }
}
