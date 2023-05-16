namespace Nett.Core;

[ExcludeFromCodeCoverage]
public class DomainException : Exception
{
    public string Code { get; }

    public DomainException(string code, string message) : base(message) =>
        Code = code;

    public DomainException(string message) : base(message) =>
        Code = string.Empty;

    public Error MapToError() =>
        new (Code, Message);

    public static void ThrowIf(bool condition, string code, string message)
    {
        if (condition)
            throw new DomainException(code, message);
    }

    public static void ThrowIfNull(object value, string code, string message)
    {
        if (value is null)
            throw new DomainException(code, message);
    }

    public static void ThrowIfEmpty(Guid value, string code, string message)
    {
        if (Guid.Empty.Equals(value))
            throw new DomainException(code, message);
    }

    public static void ThrowIfNullOrEmpty(string value, string code, string message)
    {
        if (string.IsNullOrEmpty(value))
            throw new DomainException(code, message);
    }

    public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> values, string code, string message)
    {
        if (values is null || !values.Any())
            throw new DomainException(code, message);
    }

    public static void ThrowIfEmpty<T>(IEnumerable<T> values, string code, string message)
    {
        if (!values.Any())
            throw new DomainException(code, message);
    }

    public static void ThrowIfNegative(double value, string code, string message)
    {
        if (value < 0)
            throw new DomainException(code, message);
    }

    public static void ThrowIfNegative(decimal value, string code, string message)
    {
        if (value < 0)
            throw new DomainException(code, message);
    }
}