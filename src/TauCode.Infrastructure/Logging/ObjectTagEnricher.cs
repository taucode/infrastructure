using Serilog.Core;
using Serilog.Events;

namespace TauCode.Infrastructure.Logging;

public class ObjectTagEnricher : ILogEventEnricher
{
    protected const string PropertyName = "ObjectTag";
    private readonly Func<ObjectTag> _tagGetter;

    private ObjectTag? _lastTag;
    private LogEventProperty? _lastProperty;

    public ObjectTagEnricher(Func<ObjectTag> tagGetter)
    {
        _tagGetter = tagGetter ?? throw new ArgumentNullException(nameof(tagGetter));
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var tag = _tagGetter();

        if (tag.Equals(default))
        {
            return; // nothing to enrich with.
        }

        LogEventProperty property;
        if (_lastTag.HasValue && _lastTag.Value.Equals(tag))
        {
            property = _lastProperty!;
        }
        else
        {
            property = this.BuildProperty(tag, propertyFactory);

            _lastTag = tag;
            _lastProperty = property;
        }

        logEvent.AddPropertyIfAbsent(property);
    }

    protected virtual LogEventProperty BuildProperty(ObjectTag tag, ILogEventPropertyFactory propertyFactory)
    {
        var items = new[]
        {
            tag.Type,
            tag.Name,
            tag.State,
        };

        var text = $"({string.Join(", ", items.Where(x => x != null))})";

        var property = propertyFactory.CreateProperty(PropertyName, text);

        return property;
    }
}