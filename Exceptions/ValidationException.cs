namespace Nett.Core;

[ExcludeFromCodeCoverage]
public class ValidationException : Exception
{
    public IEnumerable<Error> Errors { get; }

    public ValidationException(IEnumerable<Error> errors) : base("Validation Exception") =>
        Errors = errors;
}

[ExcludeFromCodeCoverage]
public record Error(string Code, string Message);