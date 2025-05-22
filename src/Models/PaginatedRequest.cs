namespace Nett.Core.Models;

public class PaginatedRequest
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 20;
    public int Offset => (Page - 1) * Limit;
    public string? OrderBy { get; set; }
    public string? Direction { get; set; }
}
