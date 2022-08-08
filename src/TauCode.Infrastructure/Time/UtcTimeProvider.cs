namespace TauCode.Infrastructure.Time;

public class UtcTimeProvider : TimeProviderBase
{
    public static UtcTimeProvider Instance = new UtcTimeProvider();

    private UtcTimeProvider()
    {
    }

    public override DateTimeOffset GetCurrentTime() => DateTimeOffset.UtcNow;
}