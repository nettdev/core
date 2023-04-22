using System.Reflection;

namespace Nett.Core;

[ExcludeFromCodeCoverage]
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>> where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum?> _enumerations = CreateEnumerations();

    public string Name { get; protected set; }
    public int Value { get; protected set; }

    protected Enumeration(string name, int value) =>
        (Name, Value) = (name, value);

    public static TEnum? FromValue(int value) =>
        _enumerations.TryGetValue(value, out TEnum? enumeration) ? enumeration : null;

    public static TEnum? FromName(string name) => 
        _enumerations.Values.SingleOrDefault(e => e?.Name == name);

    public bool Equals(Enumeration<TEnum>? other) =>
        other is null ? false : GetType() == other.GetType() && Value == other.Value;

    public override bool Equals(object? obj) =>
        obj is Enumeration<TEnum> other && Equals(other);

    public override int GetHashCode() =>
        Value.GetHashCode();

    public override string ToString() => 
        Name;

    private static Dictionary<int, TEnum?> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);
        var fieldsForType = enumerationType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum?)fieldInfo.GetValue(null));

        return fieldsForType.ToDictionary(x => x?.Value ?? 0);
    }
}