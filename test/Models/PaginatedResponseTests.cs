using Nett.Core.Models;

namespace Nett.Core.UnitTest.Models;

[ExcludeFromCodeCoverage]
public class PaginatedResponseTests
{
    [Fact]
    public void ShouldReturnCorrectPageFirstItem()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 25, new PaginatedRequest { Page = 2, Limit = 5 });

        Assert.Equal(6, pagedResponse.PageFirstItem);
    }

    [Fact]
    public void ShouldReturnCorrectPageLastItem()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 25, new PaginatedRequest { Page = 2, Limit = 5 });

        Assert.Equal(10, pagedResponse.PageLastItem);
    }

    [Fact]
    public void ShouldReturnCorrectCurrentPage()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, new PaginatedRequest { Page = 1, Limit = 2 });

        Assert.Equal(1, pagedResponse.Current);
    }

    [Fact]
    public void ShouldReturnCorrectLimit()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, new PaginatedRequest { Page = 1, Limit = 2 });

        Assert.Equal(2, pagedResponse.Limit);
    }

    [Fact]
    public void ShouldReturnCorrectTotal()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, new PaginatedRequest { Page = 1, Limit = 2 });

        Assert.Equal(5, pagedResponse.Total);
    }

    [Fact]
    public void ShouldReturnTrueForHasPrevWhenCurrentIsGreaterThanOne()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, new PaginatedRequest { Page = 2, Limit = 2 });

        Assert.True(pagedResponse.HasPrev);
    }

    [Fact]
    public void ShouldReturnFalseForHasPrevWhenCurrentIsOne()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, new PaginatedRequest { Page = 1, Limit = 2 });

        Assert.False(pagedResponse.HasPrev);
    }

    [Fact]
    public void ShouldReturnTrueForHasNextWhenCurrentIsLessThanPages()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, new PaginatedRequest { Page = 1, Limit = 2 });

        Assert.True(pagedResponse.HasNext);
    }

    [Fact]
    public void ShouldReturnFalseForHasNextWhenCurrentIsEqualToPages()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, new PaginatedRequest { Page = 3, Limit = 2 });

        Assert.False(pagedResponse.HasNext);
    }

    [Fact]
    public void ShouldReturnCorrectItems()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, new PaginatedRequest { Page = 1, Limit = 2 });

        Assert.Equal(items, pagedResponse.Items);
    }
}
