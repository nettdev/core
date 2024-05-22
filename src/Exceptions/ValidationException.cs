namespace Nett.Core;

[ExcludeFromCodeCoverage]
public class ValidationException(IEnumerable<ErrorDetails> errors) : Exception("Validation Exception")
{
    public IEnumerable<ErrorDetails> Errors { get; } = errors;
}
