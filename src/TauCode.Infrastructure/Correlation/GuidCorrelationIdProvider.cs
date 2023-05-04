namespace TauCode.Infrastructure.Correlation;

public class GuidCorrelationIdProvider : ICorrelationIdProvider
{
    public string Create()
    {
        return Guid.NewGuid().ToString();
    }
}