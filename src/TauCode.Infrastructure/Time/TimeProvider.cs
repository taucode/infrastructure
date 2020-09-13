using System;

namespace TauCode.Infrastructure.Time
{
    public static class TimeProvider
    {
        private static ITimeProvider _current;
        private static readonly object Lock;

        static TimeProvider()
        {
            Lock = new object();
            Reset();
        }

        public static DateTimeOffset GetCurrent()
        {
            lock (Lock)
            {
                return _current.GetCurrent();
            }
        }

        public static void Override(ITimeProvider timeProvider)
        {
            lock (Lock)
            {
                _current = timeProvider;
            }
        }

        public static void Override(DateTimeOffset moment)
        {
            Override(new ConstTimeProvider(moment));
        }

        public static void Reset()
        {
            lock (Lock)
            {
                _current = new UtcTimeProvider();
            }
        }
    }
}
