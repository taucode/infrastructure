using System;

namespace TauCode.Infrastructure
{
    public class UtcTimeProvider : ITimeProvider
    {
        public DateTime GetCurrent()
        {
            return DateTime.UtcNow;
        }
    }
}
