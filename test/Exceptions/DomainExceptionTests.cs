using Nett.Core.Exceptions;

namespace Nett.Core.UnitTest.Exceptions;

[ExcludeFromCodeCoverage]
public class DomainExceptionTests
{
    [Fact]
    public void ThrowIfNull_WithNullValue_ShouldThrowsDomainException()
    {
        //Arrange
        var value = null as string;
        var message = "Value cannot be null";
        var property = "MyProperty";

        //Act
        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfNull(value, message, property));

        //Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfNull_WithoutNullValue_ShouldNotThrowsDomainException()
    {
        //Arrange
        var value = new object();
        var message = "Value cannot be null";
        var property = "MyProperty";

        //Act
        var exception = Record.Exception(() => DomainException.ThrowIfNull(value, message, property));
        
        //Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ThrowIfNullOrEmpty_WithNullOrEmptyString_ShouldThrowDomainException(string? value)
    {
        //Arrange
        var message = "Value cannot be null or empty";
        var property = "MyProperty";

        //Act
        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfNullOrEmpty(value, message, property));

        //Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfNullOrEmpty_WithValidValue_ShouldNotThrowDomainException()
    {
        //Arrange
        var value = "valid value";
        var message = "Value cannot be null or empty";
        var property = "MyProperty";

        //Act
        var exception = Record.Exception(() => DomainException.ThrowIfNullOrEmpty(value, message, property));
        
        //Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfNullOrWhiteSpace_ThrowsDomainException_WhenValueIsNullOrWhiteSpace()
    {
        //Arrange
        var value = "   ";
        var message = "Value cannot be null or whitespace";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfNullOrWhiteSpace(value, message, property));

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfEmptyGuid_ThrowsDomainException_WhenValueIsGuidEmpty()
    {
        //Arrange
        var value = Guid.Empty;
        var message = "Guid cannot be empty";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfEmpty(value, message, property));

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfEmptyDateTime_ThrowsDomainException_WhenValueIsDefault()
    {
        //Arrange
        var value = default(DateTime);
        var message = "Date time cannot be default";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfEmpty(value, message, property));

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfNullOrEmptyEnumerable_ThrowsDomainException_WhenValuesIsNull()
    {
        var values = null as IEnumerable<string>;
        var message = "Enumerable cannot be null";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfNullOrEmpty(values, message, property));

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfNullOrEmptyEnumerable_ThrowsDomainException_WhenValuesAreEmpty()
    {
        var values = Array.Empty<string>();
        var message = "Enumerable cannot be empty";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfNullOrEmpty(values, message, property));

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfEmptyEnumerable_ThrowsDomainException_WhenValuesAreEmpty()
    {
        var values = Array.Empty<string>();
        var message = "Enumerable cannot be empty";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfEmpty(values, message, property));

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfNegative_ThrowsDomainException_WhenValueIsLessThanZero()
    {
        var value = -1;
        var message = "Value cannot be negative";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfNegative(value, message, property));

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfZero_ThrowsDomainException_WhenValueIsZero()
    {
        var value = 0;
        var message = "Value cannot be zero";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfZero(value, message, property));

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfLessThan_ThrowsDomainException_WhenValueIsLessThanParameter()
    {
        var value = 1;
        var parameter = 2;
        var message = "Value cannot be less than the specified parameter";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfLessThan(value, parameter, message, property));

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfGreaterThan_ThrowsDomainException_WhenValueIsGreaterThanParameter()
    {
        var value = 2;
        var parameter = 1;
        var message = "Value cannot be greater than the specified parameter";
        var property = "MyProperty";

        var exception = Assert.Throws<DomainException>(() => DomainException.ThrowIfGreaterThan(value, parameter, message, property));

        Assert.Equal(message, exception.Message);
    }
}