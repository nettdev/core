using FluentAssertions;
using NSubstitute;
using Nett.Core.Result;

namespace Nett.Core.UnitTest.Results;

public class ResultTests
{
    private const string SuccessValue = "SuccessData";
    private static readonly Error CommonError = new() { Message = "Something went wrong.", Code = "Test.Error" };
    private static readonly Error CustomErr = new() { Message = "Custom error occurred", Code = "500" };

    [Fact]
    public void Error_Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        var message = "Test Message";
        var code = "Test.Code";
        var innerErrors = new List<Error> { new() { Message = "Inner" } };

        // Act
        var error = new Error { Message = message, Code = code, InnerErrors = innerErrors };

        // Assert
        error.Should().NotBeNull();
        error.Message.Should().Be(message);
        error.Code.Should().Be(code);
        error.InnerErrors.Should().BeEquivalentTo(innerErrors);
    }

    [Fact]
    public void Error_Database_CreatesCorrectError()
    {
        // Arrange
        var message = "DB connection failed.";
        var code = "Database.ConnectionError";

        // Act
        var error = Error.Database(message, code);

        // Assert
        error.Should().NotBeNull();
        error.Message.Should().Be(message);
        error.Code.Should().Be(code);
        error.InnerErrors.Should().BeEmpty();
    }

    [Fact]
    public void Error_NotFound_CreatesCorrectError()
    {
        // Arrange
        var message = "User not found.";
        var code = "User.NotFound";

        // Act
        var error = Error.NotFound(message, code);

        // Assert
        error.Should().NotBeNull();
        error.Message.Should().Be(message);
        error.Code.Should().Be(code);
        error.InnerErrors.Should().BeEmpty();
    }

    [Fact]
    public void Error_Validation_CreatesCorrectError()
    {
        // Arrange
        var message = "Invalid input.";
        var code = "Input.Invalid";

        // Act
        var error = Error.Validation(message, code);

        // Assert
        error.Should().NotBeNull();
        error.Message.Should().Be(message);
        error.Code.Should().Be(code);
        error.InnerErrors.Should().BeEmpty();
    }

    [Fact]
    public void Result_Success_SetsIsSuccessAndValue()
    {
        // Act
        var result = Result<string, Error>.Success(SuccessValue);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(SuccessValue);
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Result_Failure_SetsIsFailureAndError()
    {
        // Act
        var result = Result<string, Error>.Failure(CustomErr);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Error.Should().Be(CustomErr);
    }

    [Fact]
    public void Result_ImplicitOperatorFromValue_CreatesSuccessResult()
    {
        // Act
        Result<string, Error> result = SuccessValue;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(SuccessValue);
    }

    [Fact]
    public void Result_ImplicitOperatorFromError_CreatesFailureResult()
    {
        // Act
        Result<string, Error> result = CustomErr;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CustomErr);
    }

    [Fact]
    public void Result_Match_SuccessScenario_ExecutesSuccessFunc()
    {
        // Arrange
        var result = Result<string, Error>.Success(SuccessValue);
        var successFunc = Substitute.For<Func<string, int>>();
        var errorFunc = Substitute.For<Func<Error, int>>();

        successFunc.Invoke(SuccessValue).Returns(1);
        errorFunc.Invoke(Arg.Any<Error>()).Returns(0);

        // Act
        var matchResult = result.Match(successFunc, errorFunc);

        // Assert
        matchResult.Should().Be(1);
        successFunc.Received(1).Invoke(SuccessValue);
        errorFunc.DidNotReceive().Invoke(Arg.Any<Error>());
    }

    [Fact]
    public void Result_Match_FailureScenario_ExecutesErrorFunc()
    {
        // Arrange
        var result = Result<string, Error>.Failure(CustomErr);
        var successFunc = Substitute.For<Func<string, int>>();
        var errorFunc = Substitute.For<Func<Error, int>>();

        successFunc.Invoke(Arg.Any<string>()).Returns(1);
        errorFunc.Invoke(CustomErr).Returns(0);

        // Act
        var matchResult = result.Match(successFunc, errorFunc);

        // Assert
        matchResult.Should().Be(0);
        successFunc.DidNotReceive().Invoke(Arg.Any<string>());
        errorFunc.Received(1).Invoke(CustomErr);
    }

    [Fact]
    public void ResultGeneric_Success_SetsIsSuccessAndValue()
    {
        // Act
        var result = Result<string>.Success(SuccessValue);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(SuccessValue);
        result.Error.Should().BeNull();
    }

    [Fact]
    public void ResultGeneric_Failure_SetsIsFailureAndError()
    {
        // Act
        var result = Result<string>.Failure(CommonError);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Error.Should().Be(CommonError);
    }

    [Fact]
    public void ResultGeneric_ImplicitOperatorFromValue_CreatesSuccessResult()
    {
        // Act
        Result<string> result = SuccessValue;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(SuccessValue);
    }

    [Fact]
    public void ResultGeneric_ImplicitOperatorFromError_CreatesFailureResult()
    {
        // Act
        Result<string> result = CommonError;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CommonError);
    }

    [Fact]
    public void ResultGeneric_Match_SuccessScenario_ExecutesSuccessFunc()
    {
        // Arrange
        var result = Result<string>.Success(SuccessValue);
        var successFunc = Substitute.For<Func<string, int>>();
        var errorFunc = Substitute.For<Func<Error, int>>();

        successFunc.Invoke(SuccessValue).Returns(1);
        errorFunc.Invoke(Arg.Any<Error>()).Returns(0);

        // Act
        var matchResult = result.Match(successFunc, errorFunc);

        // Assert
        matchResult.Should().Be(1);
        successFunc.Received(1).Invoke(SuccessValue);
        errorFunc.DidNotReceive().Invoke(Arg.Any<Error>());
    }

    [Fact]
    public void ResultGeneric_Match_FailureScenario_ExecutesErrorFunc()
    {
        // Arrange
        var result = Result<string>.Failure(CommonError);
        var successFunc = Substitute.For<Func<string, int>>();
        var errorFunc = Substitute.For<Func<Error, int>>();

        successFunc.Invoke(Arg.Any<string>()).Returns(1);
        errorFunc.Invoke(CommonError).Returns(0);

        // Act
        var matchResult = result.Match(successFunc, errorFunc);

        // Assert
        matchResult.Should().Be(0);
        successFunc.DidNotReceive().Invoke(Arg.Any<string>());
        errorFunc.Received(1).Invoke(CommonError);
    }
}