using Nett.Core.Models;

namespace Nett.Core.UnitTest.Models;

[ExcludeFromCodeCoverage]
public class PaginatedRequestTests
{
    [Theory]
    [InlineData(1, 10, "name", "ASC", 0)]
    [InlineData(2, 10, "age", "DESC", 10)]
    [InlineData(3, 50, "salary", "ASC", 100)]
    public void ConstructorSetsPropertiesCorrectly(int page, int limit, string orderBy, string direction, int offset)
    {
        //Act
        var pagedRequest = new PaginatedRequest
        {
            Page = page,
            Limit = limit,
            OrderBy = orderBy,
            Direction = direction
        };

        //Assert
        Assert.Equal(page, pagedRequest.Page);
        Assert.Equal(limit, pagedRequest.Limit);
        Assert.Equal(orderBy, pagedRequest.OrderBy);
        Assert.Equal(direction, pagedRequest.Direction);
        Assert.Equal(offset, pagedRequest.Offset);
    }
}
