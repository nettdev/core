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

    public static void ThrowIf(bool condition, string message, string code = "")
    {
        if (condition)
            throw new DomainException(code, message);
    }

    public static void ThrowIfNull(object value, string message, string code = "")
    {
        if (value is null)
            throw new DomainException(code, message);
    }

    public static void ThrowIfNullOrEmpty(string value, string message, string code = "")
    {
        if (string.IsNullOrEmpty(value))
            throw new DomainException(code, message);
    }

    public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> values, string message, string code = "")
    {
        if (values is null || !values.Any())
            throw new DomainException(code, message);
    }

    public static void ThrowIfEmpty<T>(IEnumerable<T> values, string message, string code = "")
    {
        if (!values.Any())
            throw new DomainException(code, message);
    }

    public static void ThrowIfNegative(double value, string message, string code = "")
    {
        if (value < 0)
            throw new DomainException(code, message);
    }
}