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
}