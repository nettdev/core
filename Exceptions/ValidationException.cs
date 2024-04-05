namespace Nett.Core;

[ExcludeFromCodeCoverage]
public class ValidationException : Exception
{
    public IEnumerable<ErrorDetails> Errors { get; }

    public ValidationException(IEnumerable<ErrorDetails> errors) : base("Validation Exception") =>
        Errors = errors;
}
