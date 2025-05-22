namespace Nett.Core.Result;

[ExcludeFromCodeCoverage]
public record ErrorDetails(IEnumerable<string> Messages)
{
    public static implicit operator ErrorDetails(string error) => new([error]);
}

[ExcludeFromCodeCoverage]
public sealed class Error(string title, ErrorDetails errors)
{
    public string Title => title;
    public ErrorDetails Errors => errors;
    
    public static Error DefaultDatabaseError =>
        new("An error occurred while saving", "DbConnection");
}
