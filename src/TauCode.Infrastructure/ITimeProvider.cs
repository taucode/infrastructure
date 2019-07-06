using System;

namespace TauCode.Infrastructure
{
    public interface ITimeProvider
    {
        DateTime GetCurrent();
    }
}