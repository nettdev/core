namespace Nett.Core.Result;

[ExcludeFromCodeCoverage]
public sealed class Error
{
    public required string Message { get; init; }
    public string Code { get; init; } = "Generic.Error";
    public ErrorType Type { get; init; } = ErrorType.Validation;
    public IReadOnlyList<Error> InnerErrors { get; init; } = [];

    public static implicit operator Error(string message) =>
        new() { Message = message };
}

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict
}
