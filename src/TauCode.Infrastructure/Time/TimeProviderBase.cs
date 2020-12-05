using System;

namespace TauCode.Infrastructure.Time
{
    public abstract class TimeProviderBase : ITimeProvider
    {
        public abstract DateTimeOffset GetCurrentTime();

        public DateTimeOffset GetCurrentDate() => this.GetCurrentTime().UtcDateTime.Date;
    }
}
