namespace Nett.Core;

[ExcludeFromCodeCoverage]
public class DomainException : Exception
{
    private const string Severity = "Error";
    
    public string? Code { get; }
    public string? Property { get; }

    public DomainException(string message, string? property = null, string? code = null) : base(message) =>
        (Property, Code) = (property, code);

    public ErrorDetails MapToError() =>
        new (Message, Property, Code, Severity);

    public static void ThrowIfNull(object value, string message, string? property, string? code) =>
        ThrowIf(value is null, message, property, code);

    public static void ThrowIfNullOrEmpty(string value, string message, string? property, string? code) =>
        ThrowIf(string.IsNullOrEmpty(value), message, property, code);

    public static void ThrowIfNullOrWhiteSpace(string value, string message, string? property, string? code) =>
        ThrowIf(string.IsNullOrWhiteSpace(value), message, property, code);

    public static void ThrowIfEmpty(Guid value, string message, string? property, string? code) =>
        ThrowIf(Guid.Empty.Equals(value), message, property, code);

    public static void ThrowIfEmpty(DateTime value, string message, string? property, string? code) =>
        ThrowIf(default(DateTime).Equals(value), message, property, code);

    public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> values, string message, string? property, string? code) =>
        ThrowIf(values is null || !values.Any(), message, property, code);

    public static void ThrowIfEmpty<T>(IEnumerable<T> values, string message, string? property, string? code) =>
        ThrowIf(!values.Any(), message, property, code);

    public static void ThrowIfNegative(int value, string message, string? property, string? code) =>
        ThrowIf(value < 0, message, property, code);

    public static void ThrowIfNegative(double value, string message, string? property, string? code) =>
        ThrowIf(value < 0, message, property, code);

    public static void ThrowIfNegative(decimal value, string message, string? property, string? code) =>
        ThrowIf(value < 0, message, property, code);

    public static void ThrowIfZero(int value, string message, string? property, string? code) =>
        ThrowIf(value == 0, message, property, code);

    public static void ThrowIfZero(double value, string message, string? property, string? code) =>
        ThrowIf(value == 0, message, property, code);

    public static void ThrowIfZero(decimal value, string message, string? property, string? code) =>
        ThrowIf(value == 0, message, property, code);

    public static void ThrowIfLessThan(int value, double parameter, string message, string? property, string? code) =>
        ThrowIf(value < parameter, message, property, code);

    public static void ThrowIfLessThan(double value, double parameter, string message, string? property, string? code) =>
        ThrowIf(value < parameter, message, property, code);

    public static void ThrowIfLessThan(decimal value, decimal parameter, string message, string? property, string? code) =>
        ThrowIf(value < parameter, message, property, code);

    public static void ThrowIfGreaterThan(int value, double parameter, string message, string? property, string? code) =>
        ThrowIf(value > parameter, message, property, code);

    public static void ThrowIfGreaterThan(double value, double parameter, string message, string? property, string? code) =>
        ThrowIf(value > parameter, message, property, code);

    public static void ThrowIfGreaterThan(decimal value, decimal parameter, string message, string? property, string? code) =>
        ThrowIf(value > parameter, message, property, code);

    public static void ThrowIf(bool condition, string message, string? property, string? code)
    {
        if (condition)
            throw new DomainException(message, property, code);
    }
}