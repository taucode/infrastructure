using System;

namespace TauCode.Infrastructure.Time
{
    public interface ITimeProvider
    {
        DateTimeOffset GetCurrentTime();

        DateTimeOffset GetCurrentDate();
    }
}