namespace Nett.Core.Models;

public class PaginatedRequest(int page, int limit, string orderBy, string direction = "asc")
{
    public int Page => page;
    public int Limit => limit;
    public int Offset => (page - 1) * limit;
    public string OrderBy => orderBy;
    public string Direction = direction;
}
