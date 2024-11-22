namespace Nett.Core;

public readonly record struct DateRange
{
    public DateOnly From { get; }
    public DateOnly To { get; }
    public readonly int Days => To.DayNumber - From.DayNumber;
    public readonly IEnumerable<DateOnly> Dates => Enumerable.Range(0, Days).Select(From.AddDays);

    public DateRange(DateOnly from, DateOnly to) => (From, To) = (from, to);
}
