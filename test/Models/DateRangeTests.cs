using Nett.Core.Models;

namespace Nett.Core.UnitTest.Models;

[ExcludeFromCodeCoverage]
public sealed class DateRangeTests
{
    [Fact]
    public void ConstructWithValidDates()
    {
        var from = new DateOnly(2023, 1, 1);
        var to = new DateOnly(2023, 1, 5);

        var dateRange = new DateRange(from, to);

        Assert.Equal(from, dateRange.From);
        Assert.Equal(to, dateRange.To);
        Assert.Equal(4, dateRange.Days); // 5 - 1 = 4 days
    }

    [Fact]
    public void ConstructWithSameDate()
    {
        var from = new DateOnly(2023, 1, 1);
        var to = new DateOnly(2023, 1, 1);

        var dateRange = new DateRange(from, to);

        Assert.Equal(from, dateRange.From);
        Assert.Equal(to, dateRange.To);
        Assert.Equal(0, dateRange.Days); // No days between the same date
    }

    [Fact]
    public void ConstructWithNegativeDays()
    {
        var from = new DateOnly(2023, 1, 5);
        var to = new DateOnly(2023, 1, 1);

        Assert.Throws<ArgumentException>(() => new DateRange(from, to));
    }

    [Fact]
    public void ConstructWithDaysGreaterThanOne()
    {
        var from = new DateOnly(2023, 1, 1);
        var to = new DateOnly(2023, 1, 5);

        var dateRange = new DateRange(from, to);

        Assert.Equal(4, dateRange.Days); // 5 - 1 = 4 days
    }

    [Fact]
    public void DaysPropertyReturnsCorrectly()
    {
        var from = new DateOnly(2023, 1, 1);
        var to = new DateOnly(2023, 1, 5);

        var dateRange = new DateRange(from, to);

        Assert.Equal(4, dateRange.Days); // 5 - 1 = 4 days
    }

    [Fact]
    public void DatesPropertyReturnsCorrectly()
    {
        var from = new DateOnly(2023, 1, 1);
        var to = new DateOnly(2023, 1, 5);

        var dateRange = new DateRange(from, to);

        var expectedDates = new List<DateOnly> { new DateOnly(2023, 1, 1), new DateOnly(2023, 1, 2), new DateOnly(2023, 1, 3), new DateOnly(2023, 1, 4) };
        var actualDates = dateRange.Dates.ToList();

        Assert.Equal(expectedDates, actualDates);
    }
}
