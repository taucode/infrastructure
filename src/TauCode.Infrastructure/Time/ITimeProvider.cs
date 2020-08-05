using System;

namespace TauCode.Infrastructure.Time
{
    public interface ITimeProvider
    {
        DateTime GetCurrent();
    }
}