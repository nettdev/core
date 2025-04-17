using FluentValidation.Results;
using Nett.Core.Result;

namespace Nett.Core.Extensions;

public static class ValidationResultExtensions
{
    private const string ErrorType = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    private const string ErrorTitle = "One or more validation errors occurred.";

    public static Error MapToError(this ValidationResult validationResult)
    {
        var errorDetails = validationResult.Errors.Select(CreateErrorDetails);
        return new Error(ErrorType, ErrorTitle, errorDetails);
    }

    private static ErrorDetails CreateErrorDetails(ValidationFailure failure) =>
        new(failure.ErrorMessage, failure.PropertyName, failure.ErrorCode, failure.Severity.ToString());
}