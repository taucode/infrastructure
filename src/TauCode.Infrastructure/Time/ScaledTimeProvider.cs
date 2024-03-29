﻿using System.Diagnostics;

namespace TauCode.Infrastructure.Time;

public class ScaledTimeProvider : TimeProviderBase
{
    private readonly DateTimeOffset _initial;
    private readonly double _scale;
    private readonly Stopwatch _stopwatch;

    public ScaledTimeProvider(DateTimeOffset? initial = null, double scale = 1.0)
    {
        _stopwatch = Stopwatch.StartNew();
        _initial = (initial ?? DateTimeOffset.UtcNow);
        _scale = scale;
    }

    public override DateTimeOffset GetCurrentTime()
    {
        var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        var scaleElapsed = _scale * elapsedMilliseconds;
        var current = _initial.AddMilliseconds(scaleElapsed);
        return current;
    }
}