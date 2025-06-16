namespace Nett.Core.Result;

[ExcludeFromCodeCoverage]
public sealed class Error
{
    public required string Message { get; init; }
    public string? Code { get; init; }
    public List<Error> InnerErrors { get; init; } = [];

    public static Error Database(string message, string code = "Database.Error") => new() { Code = code, Message = message };
    public static Error NotFound(string message, string code = "Entry.NotFound") => new() { Code = code, Message = message };
    public static Error Validation(string message, string code = "Validation.Fail") => new() { Code = code, Message = message };

    public static implicit operator Error(string message) => new() { Message = message, Code = "Generic.Error" };
}
