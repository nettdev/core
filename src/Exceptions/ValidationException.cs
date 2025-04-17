using Nett.Core.Result;

namespace Nett.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class ValidationException(IEnumerable<ErrorDetails> errors) : Exception("Validation Exception")
{
    public IEnumerable<ErrorDetails> Errors { get; } = errors;
}
