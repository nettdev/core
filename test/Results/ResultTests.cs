using Nett.Core.Result;

namespace Nett.Core.UnitTest.Results;

[ExcludeFromCodeCoverage]
public class ResultTests
{
    [Fact]
    public void WithErrorInstance_ShouldReturnIsFailureTrue()
    {
        //Arrange 
        var error = new Error("", "");
        
        //Act
        Result<Guid, Error> result = error;

        //Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void WithDataInstance_ShouldReturnIsSuccessTrue()
    {
        //Arrange && Act
        Result<Guid, Error> result = Guid.NewGuid();

        //Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Match_WithErrorInstance_ShouldReturnIsFailureTrue()
    {
        //Arrange 
        var error = new Error("", "");
        Result<Guid, Error> result = error;
        
        //Act
        var matchResult = result.Match(success: (data) => false, error: (error) => true);

        //Assert
        Assert.True(matchResult);
    }

    [Fact]
    public void Match_WithDataInstance_ShouldReturnTrue()
    {
        //Arrange 
        var error = new Error("", "");
        Result<Guid, Error> result = Guid.NewGuid();
        
        //Act
        var matchResult = result.Match(success: (data) => true, error: (error) => false);

        //Assert
        Assert.True(matchResult);
    }
}