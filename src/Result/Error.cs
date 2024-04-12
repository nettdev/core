namespace Nett.Core;

[ExcludeFromCodeCoverage]
public record ErrorDetails(string Message, string? Property = null, string? Code = null, string? Severity = null);

[ExcludeFromCodeCoverage]
public record Error(string Type = "", string Title = "", int StatusCode = 400, string TraceId = "", IEnumerable<ErrorDetails> Errors = null!)
{
    private const string BadRequesRFC = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";

    public static Error DefaultDatabaseError =>
        new(BadRequesRFC, "BadRequest", 400, "", [new("An error occurred while saving")]);
}
