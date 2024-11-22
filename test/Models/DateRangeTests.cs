using FluentAssertions;

namespace Nett.Core.UnitTest;

[ExcludeFromCodeCoverage]
public sealed class DateRangeTests
{
    [Theory]
    [MemberData(nameof(GetData))]
    public void CreateWithTwoDates_ShouldReturnDaysCount(DateOnly from, DateOnly to, int expected)
    {
        //Arrange & Act
        var dateRange = new DateRange(from, to);
        
        //Assert
        dateRange.Days.Should().Be(expected);
    }

    #pragma warning disable CA2211 // Non-constant fields should not be visible
    public static IEnumerable<TheoryDataRow<DateOnly, DateOnly, int>> GetData =
    [
        new(new DateOnly(2000, 01, 01), new DateOnly(2000, 01, 01), 0),
        new(new DateOnly(2000, 01, 01), new DateOnly(2000, 01, 02), 1),
        new(new DateOnly(2000, 01, 01), new DateOnly(2000, 01, 05), 4),
        new(new DateOnly(2000, 01, 01), new DateOnly(2000, 01, 10), 9),
        new(new DateOnly(2000, 01, 01), new DateOnly(2000, 01, 20), 19)
    ];
}
