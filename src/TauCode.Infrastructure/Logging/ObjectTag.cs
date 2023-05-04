namespace TauCode.Infrastructure.Logging;

public readonly struct ObjectTag : IEquatable<ObjectTag>
{
    public ObjectTag(string? type, string? name, string? state)
    {
        this.Type = type;
        this.Name = name;
        this.State = state;
    }

    public readonly string? Type;
    public readonly string? Name;
    public readonly string? State;

    public bool Equals(ObjectTag other)
    {
        return
            this.Type == other.Type &&
            this.Name == other.Name &&
            this.State == other.State;
    }

    public override bool Equals(object? obj)
    {
        return
            obj is ObjectTag other &&
            this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            this.Type,
            this.Name,
            this.State);
    }
}