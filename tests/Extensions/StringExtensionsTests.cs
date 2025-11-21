using Nett.Core.Extensions;

namespace Nett.Core.UnitTest.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("firstName", "FirstName")]
    [InlineData("hello", "Hello")]
    [InlineData("Hello", "Hello")]
    [InlineData("h", "H")]
    [InlineData("123abc", "123abc")]
    public void CapitalizeFirstLetter_ReturnsExpectedResult(string? input, string? expected)
    {
        var result = input.CapitalizeFirstLetter();

        Assert.Equal(expected, result);
    }
}
