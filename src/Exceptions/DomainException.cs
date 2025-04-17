using Nett.Core.Result;

namespace Nett.Core.Exceptions;

public class DomainException : Exception
{
    private const string Severity = "Error";
    
    public string? Code { get; }
    public string? Property { get; }

    public DomainException(string message, string? property = null, string? code = null) : base(message) =>
        (Property, Code) = (property, code);

    public ErrorDetails MapToError() =>
        new (Message, Property, Code, Severity);

    public static void ThrowIfNull(object? value, string message, string? property = null, string? code = null) =>
        ThrowIf(value is null, message, property, code);

    public static void ThrowIfNullOrEmpty(string? value, string message, string? property = null, string? code = null) =>
        ThrowIf(string.IsNullOrEmpty(value), message, property, code);

    public static void ThrowIfNullOrEmpty<T>(IEnumerable<T>? values, string message, string? property = null, string? code = null) =>
        ThrowIf(values is null || !values.Any(), message, property, code);

    public static void ThrowIfNullOrWhiteSpace(string? value, string message, string? property = null, string? code = null) =>
        ThrowIf(string.IsNullOrWhiteSpace(value), message, property, code);

    public static void ThrowIfEmpty<T>(T value, string message, string? property = null, string? code = null) where T : struct =>
        ThrowIf(default(T).Equals(value), message, property, code);

    public static void ThrowIfEmpty<T>(IEnumerable<T> values, string message, string? property = null, string? code = null) =>
        ThrowIf(!values.Any(), message, property, code);

    public static void ThrowIfNegative<T>(T value, string message, string? property = null, string? code = null) where T : IComparable<T> =>
        ThrowIf(value.CompareTo(default) < 0, message, property, code);

    public static void ThrowIfZero<T>(T value, string message, string? property = null, string? code = null) where T : IComparable<T> =>
        ThrowIf(value.CompareTo(default) == 0, message, property, code);

    public static void ThrowIfLessThan<T>(T value, T parameter, string message, string? property = null, string? code = null) where T : IComparable<T> =>
        ThrowIf(value.CompareTo(parameter) < 0, message, property, code);

    public static void ThrowIfGreaterThan<T>(T value, T parameter, string message, string? property = null, string? code = null) where T : IComparable<T> =>
        ThrowIf(value.CompareTo(parameter) > 0, message, property, code);

    public static void ThrowIf(bool condition, string message, string? property = null, string? code = null)
    {
        if (condition)
            throw new DomainException(message, property, code);
    }
}