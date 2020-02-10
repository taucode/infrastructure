using System;
using System.Diagnostics;

namespace TauCode.Infrastructure.Time
{
    public class ScaledTimeProvider : ITimeProvider
    {
        private readonly DateTime _initial;
        private readonly double _scale;
        private readonly Stopwatch _stopwatch;

        public ScaledTimeProvider(DateTime? initial = null, double scale = 1.0)
        {
            _stopwatch = Stopwatch.StartNew();
            _initial = (initial ?? DateTime.UtcNow);
            _scale = scale;
        }

        public DateTime GetCurrent()
        {
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            var scaleElapsed = _scale * elapsedMilliseconds;
            var current = _initial.AddMilliseconds(scaleElapsed);
            return current;
        }
    }
}
