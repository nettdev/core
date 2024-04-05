namespace Nett.Core;

[ExcludeFromCodeCoverage]
public class DomainException : Exception
{
    public string? Code { get; }
    public string? Property { get; }
    public string? Severity { get; }

    public DomainException(string message, string? code, string? property, string? severity) : base(message) =>
        Code = code;

    public DomainException(string message) : base(message) =>
        Code = string.Empty;

    public ErrorDetails MapToError() =>
        new (Message, Code);

    public static void ThrowIf(bool condition, string message, string? code, string? property = null, string? severity = null)
    {
        if (condition)
            throw new DomainException(message, code, property, severity);
    }

    public static void ThrowIfNull(object value,string message, string? code, string? property = null, string? severity = null)
    {
        if (value is null)
            throw new DomainException(message, code, property, severity);
    }

    public static void ThrowIfEmpty(Guid value,string message, string? code, string? property = null, string? severity = null)
    {
        if (Guid.Empty.Equals(value))
            throw new DomainException(message, code, property, severity);
    }

    public static void ThrowIfNullOrEmpty(string value,string message, string? code, string? property = null, string? severity = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(message, code, property, severity);
    }

    public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> values,string message, string? code, string? property = null, string? severity = null)
    {
        if (values is null || !values.Any())
            throw new DomainException(message, code, property, severity);
    }

    public static void ThrowIfEmpty<T>(IEnumerable<T> values,string message, string? code, string? property = null, string? severity = null)
    {
        if (!values.Any())
            throw new DomainException(message, code, property, severity);
    }

    public static void ThrowIfNegative(double value,string message, string? code, string? property = null, string? severity = null)
    {
        if (value < 0)
            throw new DomainException(message, code, property, severity);
    }

    public static void ThrowIfNegative(decimal value,string message, string? code, string? property = null, string? severity = null)
    {
        if (value < 0)
            throw new DomainException(message, code, property, severity);
    }
}