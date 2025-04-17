using Nett.Core.Models;

namespace Nett.Core.UnitTest.Models;

public class PaginatedResponseTests
{
    [Fact]
    public void ShouldReturnCorrectCurrentPage()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, 1, 2);

        Assert.Equal(1, pagedResponse.Current);
    }

    [Fact]
    public void ShouldReturnCorrectLimit()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, 1, 2);

        Assert.Equal(2, pagedResponse.Limit);
    }

    [Fact]
    public void ShouldReturnCorrectTotal()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, 1, 2);

        Assert.Equal(5, pagedResponse.Total);
    }

    [Fact]
    public void ShouldReturnTrueForHasPrevWhenCurrentIsGreaterThanOne()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, 2, 2);

        Assert.True(pagedResponse.HasPrev);
    }

    [Fact]
    public void ShouldReturnFalseForHasPrevWhenCurrentIsOne()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, 1, 2);

        Assert.False(pagedResponse.HasPrev);
    }

    [Fact]
    public void ShouldReturnTrueForHasNextWhenCurrentIsLessThanPages()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, 1, 2);

        Assert.True(pagedResponse.HasNext);
    }

    [Fact]
    public void ShouldReturnFalseForHasNextWhenCurrentIsEqualToPages()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, 3, 2);

        Assert.False(pagedResponse.HasNext);
    }

    [Fact]
    public void ShouldReturnCorrectItems()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var pagedResponse = new PaginatedResponse<int>(items, 5, 1, 2);

        Assert.Equal(items, pagedResponse.Items);
    }
}