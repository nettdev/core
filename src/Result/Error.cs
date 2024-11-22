using System.Diagnostics;

namespace Nett.Core;

[ExcludeFromCodeCoverage]
public record ErrorDetails(string Message, string? Property = null, string? Code = null, string? Severity = null);

[ExcludeFromCodeCoverage]
public sealed class Error(string type, string title, IEnumerable<ErrorDetails> errors)
{
    public string Type => type;
    public string Title => title;
    public static string? TraceId => Activity.Current?.Id;
    public IEnumerable<ErrorDetails> Errors => errors;
    
    public static Error DefaultDatabaseError =>
        new("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1", "BadRequest", [ new("An error occurred while saving") ]);
}
