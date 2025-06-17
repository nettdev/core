namespace Nett.Core.Models;

[ExcludeFromCodeCoverage]
public class PaginatedRequest
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public string? OrderBy { get; set; }
    public bool OrderByDescending { get; set; }
    public string? ThenBy { get; set; }
    public bool ThenByDescending { get; set; }
}
