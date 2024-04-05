using System.Reflection;

namespace Nett.Core;

[ExcludeFromCodeCoverage]
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>> where TEnum : Enumeration<TEnum>
{
    public string Name { get; protected set; }
    public int Value { get; protected set; }

    protected Enumeration(string name, int value) =>
        (Name, Value) = (name, value);

    public bool Equals(Enumeration<TEnum>? other) =>
        other is not null && GetType() == other.GetType() && Value == other.Value;

    public override bool Equals(object? obj) =>
        obj is Enumeration<TEnum> other && Equals(other);

    public override int GetHashCode() =>
        Value.GetHashCode();

    public override string ToString() => 
        Name;
}