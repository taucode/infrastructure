using System;

namespace TauCode.Infrastructure.Time
{
    public class UtcTimeProvider : ITimeProvider
    {
        public DateTimeOffset GetCurrent() => DateTimeOffset.UtcNow;
    }
}
