namespace Nett.Core.Models;

public sealed record DateRange
{
    public DateOnly From { get; private set; }
    public DateOnly To { get; private set; }
    public int Days => To.DayNumber - From.DayNumber;
    public IEnumerable<DateOnly> Dates => Enumerable.Range(0, Days).Select(From.AddDays);

    public DateRange(DateOnly from, DateOnly to)
    {
        if (from > to)
        {
            throw new ArgumentException("From should be greather than to");
        }

        From = from;
        To = to;
    }
}
