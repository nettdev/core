using Nett.Core.Result;

namespace Nett.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class ValidationException(Error error) : Exception("Validation Exception")
{
    public Error Error { get; } = error;
}
