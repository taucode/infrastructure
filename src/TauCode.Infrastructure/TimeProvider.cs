using System;

namespace TauCode.Infrastructure
{
    public static class TimeProvider
    {
        private static ITimeProvider _current;
        private static readonly object _lock;

        static TimeProvider()
        {
            _lock = new object();
            Reset();
        }

        public static DateTime GetCurrent()
        {
            lock (_lock)
            {
                return _current.GetCurrent();
            }
        }

        public static void Override(ITimeProvider timeProvider)
        {
            lock (_lock)
            {
                _current = timeProvider;
            }
        }

        public static void Override(DateTime dateTime)
        {
            Override(new ConstTimeProvider(dateTime));
        }

        public static void Reset()
        {
            lock (_lock)
            {
                _current = new UtcTimeProvider();
            }
        }
    }
}
