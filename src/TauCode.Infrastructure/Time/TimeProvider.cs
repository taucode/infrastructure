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

        public static DateTimeOffset GetCurrentTime()
        {
            lock (Lock)
            {
                return _current.GetCurrentTime();
            }
        }

        public static DateTimeOffset GetCurrentDate()
        {
            lock (Lock)
            {
                return _current.GetCurrentDate();
            }
        }

        public static void Override(ITimeProvider timeProvider)
        {
            lock (Lock)
            {
                _current = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            }
        }

        public static void Override(DateTimeOffset moment) => Override(new ConstTimeProvider(moment));

        public static void Override(string momentString, bool checkUtc = true)
        {
            if (momentString == null)
            {
                throw new ArgumentNullException(nameof(momentString));
            }

            var moment = DateTimeOffset.Parse(momentString);
            if (moment.Offset != TimeSpan.Zero && checkUtc)
            {
                throw new ArgumentException($"'{momentString}' does not represent UTC date and time.");
            }

            Override(moment);
        }

        public static void Reset() => Override(UtcTimeProvider.Instance);
    }
}
