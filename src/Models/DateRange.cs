namespace Nett.Core;

public sealed record DateRange
{
    public DateOnly From { get; private set; }
    public DateOnly To { get; private set; }
    public int Days => To.DayNumber - From.DayNumber;
    public IEnumerable<DateOnly> Dates => Enumerable.Range(0, Days).Select(From.AddDays);

    public DateRange(DateOnly from, DateOnly to) => (From, To) = (from, to);
}
