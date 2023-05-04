namespace TauCode.Infrastructure.Correlation;

public interface ICorrelationIdProvider
{
    string Create();
}