using Nett.Core.Result;

namespace Nett.Core.Exceptions;

public class DomainException(string message, string? code = null) : Exception(message)
{
    public string? Code { get; } = code;

    public Error MapToError() =>
        new() { Message = Message, Code = Code };

    public static void ThrowIfNull(object? value, string message, string? code = null) =>
        ThrowIf(value is null, message, code);

    public static void ThrowIfNullOrEmpty(string? value, string message, string? code = null) =>
        ThrowIf(string.IsNullOrEmpty(value), message, code);

    public static void ThrowIfNullOrEmpty<T>(IEnumerable<T>? values, string message, string? code = null) =>
        ThrowIf(values is null || !values.Any(), message, code);

    public static void ThrowIfNullOrWhiteSpace(string? value, string message, string? code = null) =>
        ThrowIf(string.IsNullOrWhiteSpace(value), message, code);

    public static void ThrowIfEmpty<T>(T value, string message, string? code = null) where T : struct =>
        ThrowIf(default(T).Equals(value), message, code);

    public static void ThrowIfEmpty<T>(IEnumerable<T> values, string message, string? code = null) =>
        ThrowIf(!values.Any(), message, code);

    public static void ThrowIfNegative<T>(T value, string message, string? code = null) where T : IComparable<T> =>
        ThrowIf(value.CompareTo(default) < 0, message, code);

    public static void ThrowIfZero<T>(T value, string message, string? code = null) where T : IComparable<T> =>
        ThrowIf(value.CompareTo(default) == 0, message, code);

    public static void ThrowIfLessThan<T>(T value, T parameter, string message, string? code = null) where T : IComparable<T> =>
        ThrowIf(value.CompareTo(parameter) < 0, message, code);

    public static void ThrowIfGreaterThan<T>(T value, T parameter, string message, string? code = null) where T : IComparable<T> =>
        ThrowIf(value.CompareTo(parameter) > 0, message, code);

    public static void ThrowIf(bool condition, string message, string? code = null)
    {
        if (condition)
            throw new DomainException(message, code);
    }
}
