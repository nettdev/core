using FluentValidation;
using FluentValidation.Results;
using Nett.Core.Extensions;

namespace Nett.Core.UnitTest.Extensions;

public class ValidationResultExtensionsTests
{
    [Fact]
    public void MapToError_ReturnsCorrectErrorTypeTitleAndEmptyDetails()
    {
        // Arrange
        var validationResult = new ValidationResult(new List<ValidationFailure>());
        
        // Act
        var error = validationResult.MapToError();

        // Assert
        Assert.Equal("https://tools.ietf.org/html/rfc7231#section-6.5.1", error.Type);
        Assert.Equal("One or more validation errors occurred.", error.Title);
        Assert.Empty(error.Errors);
    }

    [Fact]
    public void MapToError_ReturnsCorrectErrorTypeTitleAndSingleDetail()
    {
        // Arrange
        var failure = new ValidationFailure("Property1", "The property is invalid");
        var validationResult = new ValidationResult([failure]);

        // Act
        var error = validationResult.MapToError();

        // Assert
        Assert.Equal("https://tools.ietf.org/html/rfc7231#section-6.5.1", error.Type);
        Assert.Equal("One or more validation errors occurred.", error.Title);
        Assert.Single(error.Errors);
        Assert.Contains(failure.ErrorMessage, error.Errors.First().Message);
    }

    [Fact]
    public void MapToError_ReturnsCorrectErrorTypeTitleAndMultipleDetails()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Property1", "The property is invalid"),
            new ValidationFailure("Property2", "Another error message")
        };
        var validationResult = new ValidationResult(failures);

        // Act
        var error = validationResult.MapToError();

        // Assert
        Assert.Equal("https://tools.ietf.org/html/rfc7231#section-6.5.1", error.Type);
        Assert.Equal("One or more validation errors occurred.", error.Title);
        Assert.Equal(failures.Count, error.Errors.Count());
        foreach (var failure in failures)
        {
            Assert.Contains(failure.ErrorMessage, error.Errors.Select(x => x.Message));
        }
    }

    [Fact]
    public void MapToError_ReturnsCorrectErrorTypeTitleAndSeverityDetails()
    {
        // Arrange
        var failure = new ValidationFailure("Property1", "The property is invalid") { Severity = Severity.Warning };
        var validationResult = new ValidationResult([failure]);

        // Act
        var error = validationResult.MapToError();

        // Assert
        Assert.Equal("https://tools.ietf.org/html/rfc7231#section-6.5.1", error.Type);
        Assert.Equal("One or more validation errors occurred.", error.Title);
        Assert.Single(error.Errors);
        var details = error.Errors.First();
        Assert.Contains(failure.ErrorMessage, details.Message);
        Assert.Equal("Warning", details.Severity!.ToString());
    }
}