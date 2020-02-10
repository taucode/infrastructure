using System;

namespace TauCode.Infrastructure.Time
{
    public class UtcTimeProvider : ITimeProvider
    {
        public DateTime GetCurrent()
        {
            return DateTime.UtcNow;
        }
    }
}
