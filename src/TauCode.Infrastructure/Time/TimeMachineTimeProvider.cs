﻿namespace TauCode.Infrastructure.Time;

public class TimeMachineTimeProvider : ShiftedTimeProvider
{
    public TimeMachineTimeProvider(DateTimeOffset @base)
        : base(@base - DateTimeOffset.UtcNow)
    {
        this.Base = @base;
    }

    public DateTimeOffset Base { get; }

    public async Task<bool> WaitUntil(
        DateTimeOffset fakeUntil,
        CancellationToken cancellationToken = default)
    {
        if (fakeUntil <= this.Base)
        {
            throw new ArgumentException("Cannot wait for fake past.", nameof(fakeUntil));
        }

        try
        {
            while (true)
            {
                await Task.Delay(1, cancellationToken);

                if (this.GetCurrentTime() >= fakeUntil)
                {
                    return true;
                }
            }
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    public async Task<bool> WaitUntilElapsed(
        TimeSpan timeout,
        CancellationToken token = default)
    {
        var now = this.GetCurrentTime();

        var elapsed = now - this.Base;
        if (elapsed >= timeout)
        {
            throw new InvalidOperationException("Too late.");
        }

        try
        {
            while (true)
            {
                await Task.Delay(1, token);

                now = this.GetCurrentTime();

                elapsed = now - this.Base;
                if (elapsed >= timeout)
                {
                    return true;
                }
            }
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }
}