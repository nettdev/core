namespace Nett.Core;

public record ErrorDetails(string Message, string? Code = null, string? Property = null, string? Severity = null);

public record Error(IEnumerable<ErrorDetails> Errors, string Type = "", string Title = "", int StatusCode = 400, string TraceId = "")
{
    private const string BadRequesRFC = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";

    public static Error DefaultDatabaseError =>
        new([new("An error occurred while saving")], BadRequesRFC, "BadRequest", 400);
}
