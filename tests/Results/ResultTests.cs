using NSubstitute;
using Nett.Core.Result;
using Shouldly;

namespace Nett.Core.UnitTest.Results;

[ExcludeFromCodeCoverage]
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
        var type = "Test.Type";
        var innerErrors = new List<Error> { new(){ Message = "Inner" } };

        // Act
        var error = new Error { Message = message, Code = type, InnerErrors = innerErrors };

        // Assert
        error.ShouldNotBeNull();
        error.Message.ShouldBe(message);
        error.Code.ShouldBe(type);
        error.InnerErrors.ShouldBeEquivalentTo(innerErrors);
    }

    [Fact]
    public void Result_Success_SetsIsSuccessAndValue()
    {
        // Act
        var result = Result<string, Error>.Success(SuccessValue);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Value.ShouldBe(SuccessValue);
        result.Error.ShouldBeNull();
    }

    [Fact]
    public void Result_Failure_SetsIsFailureAndError()
    {
        // Act
        var result = Result<string, Error>.Failure(CustomErr);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Value.ShouldBeNull();
        result.Error.ShouldBe(CustomErr);
    }

    [Fact]
    public void Result_ImplicitOperatorFromValue_CreatesSuccessResult()
    {
        // Act
        Result<string, Error> result = SuccessValue;

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(SuccessValue);
    }

    [Fact]
    public void Result_ImplicitOperatorFromError_CreatesFailureResult()
    {
        // Act
        Result<string, Error> result = CustomErr;

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CustomErr);
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
        matchResult.ShouldBe(1);
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
        matchResult.ShouldBe(0);
        successFunc.DidNotReceive().Invoke(Arg.Any<string>());
        errorFunc.Received(1).Invoke(CustomErr);
    }

    [Fact]
    public void ResultGeneric_Success_SetsIsSuccessAndValue()
    {
        // Act
        var result = Result<string>.Success(SuccessValue);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Value.ShouldBe(SuccessValue);
        result.Error.ShouldBeNull();
    }

    [Fact]
    public void ResultGeneric_Failure_SetsIsFailureAndError()
    {
        // Act
        var result = Result<string>.Failure(CommonError);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Value.ShouldBeNull();
        result.Error.ShouldBe(CommonError);
    }

    [Fact]
    public void ResultGeneric_ImplicitOperatorFromValue_CreatesSuccessResult()
    {
        // Act
        Result<string> result = SuccessValue;

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(SuccessValue);
    }

    [Fact]
    public void ResultGeneric_ImplicitOperatorFromError_CreatesFailureResult()
    {
        // Act
        Result<string> result = CommonError;

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CommonError);
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
        matchResult.ShouldBe(1);
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
        matchResult.ShouldBe(0);
        successFunc.DidNotReceive().Invoke(Arg.Any<string>());
        errorFunc.Received(1).Invoke(CommonError);
    }
}