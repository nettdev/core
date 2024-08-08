using System.Diagnostics;

namespace Nett.Core;

[ExcludeFromCodeCoverage]
public record ErrorDetails(string Message, string? Property = null, string? Code = null, string? Severity = null);

[ExcludeFromCodeCoverage]
public sealed class Error(string type, string title, IEnumerable<ErrorDetails> errors)
{
    public string Type => type;
    public string Title => title;
    public string? TraceId => Activity.Current?.Id;
    public IEnumerable<ErrorDetails> Errors => errors;
    
    private const string BadRequesRFC = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";

    public static Error DefaultDatabaseError =>
        new(BadRequesRFC, "BadRequest", [new("An error occurred while saving")]);
}
