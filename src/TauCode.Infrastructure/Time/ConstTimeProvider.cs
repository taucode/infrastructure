namespace TauCode.Infrastructure.Time;

public class ConstTimeProvider : TimeProviderBase
{
    private readonly DateTimeOffset _moment;

    public ConstTimeProvider(DateTimeOffset moment)
    {
        _moment = moment.ToUniversalTime();
    }

    public override DateTimeOffset GetCurrentTime() => _moment;
}