using Nett.Core.Models;

namespace Nett.Core.UnitTest.Models;

[ExcludeFromCodeCoverage]
public class PaginatedResponseTests
{
    [Fact]
    public void ShouldReturnCorrectCurrentPage()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PagedResponse<int>(items, count: 5, page: 1, limit: 2);

        Assert.Equal(1, pagedResponse.Page);
    }

    [Fact]
    public void ShouldReturnCorrectLimit()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PagedResponse<int>(items, count: 5, page: 1, limit: 2 );

        Assert.Equal(2, pagedResponse.Limit);
    }

    [Fact]
    public void ShouldReturnCorrectTotal()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PagedResponse<int>(items, count: 5, page: 1, limit: 2 );

        Assert.Equal(5, pagedResponse.TotalCount);
    }

    [Fact]
    public void ShouldReturnTrueForHasPrevWhenCurrentIsGreaterThanOne()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PagedResponse<int>(items, count: 5, page: 2, limit: 2 );

        Assert.True(pagedResponse.HasPrev);
    }

    [Fact]
    public void ShouldReturnFalseForHasPrevWhenCurrentIsOne()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PagedResponse<int>(items, count: 5, page: 1, limit: 2 );

        Assert.False(pagedResponse.HasPrev);
    }

    [Fact]
    public void ShouldReturnTrueForHasNextWhenCurrentIsLessThanPages()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PagedResponse<int>(items, count: 5, page: 1, limit: 2 );

        Assert.True(pagedResponse.HasNext);
    }

    [Fact]
    public void ShouldReturnFalseForHasNextWhenCurrentIsEqualToPages()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PagedResponse<int>(items, count: 5, page: 3, limit: 2 );

        Assert.False(pagedResponse.HasNext);
    }

    [Fact]
    public void ShouldReturnCorrectItems()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PagedResponse<int>(items, count: 5, page: 1, limit: 2 );

        Assert.Equal(items, pagedResponse.Items);
    }
}
