using System;

namespace TauCode.Infrastructure
{
    public class ShiftedTimeProvider : ITimeProvider
    {
        public ShiftedTimeProvider(TimeSpan shift)
        {
            this.Shift = shift;
        }

        public TimeSpan Shift { get; }

        public DateTime GetCurrent() => DateTime.UtcNow + this.Shift;
    }
}
