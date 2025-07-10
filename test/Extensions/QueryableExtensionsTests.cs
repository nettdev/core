using System.Linq.Expressions;
using FluentAssertions;
using Nett.Core.Domain;
using Nett.Core.Extensions;
using Nett.Core.Models;

namespace Nett.Core.UnitTest.Extensions;

[ExcludeFromCodeCoverage]
public class QueryableExtensionsTests
{
    private class TestEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    private static Expression<Func<TestEntity, TestEntity>> _map = x => x;

    private static IQueryable<TestEntity> GetTestQueryable()
    {
        return new List<TestEntity>
        {
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Charlie", Age = 30 },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Alice", Age = 25 },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Bob", Age = 35 }
        }.AsQueryable();
    }

    private static Dictionary<string, Expression<Func<TestEntity, object>>> GetSortMap()
    {
        return new Dictionary<string, Expression<Func<TestEntity, object>>>
        {
            { "name", x => x.Name },
            { "age", x => x.Age }
        };
    }

    [Fact]
    public void ApplyOrderBy_OrderByAscending_ReturnsSortedQuery()
    {
        var queryable = GetTestQueryable();
        var sortMap = GetSortMap();
        var request = new PaginatedRequest { OrderBy = "name", OrderByDescending = false };

        var result = queryable.Apply(sortMap, _map, request).ToList();

        result.Should().BeInAscendingOrder(x => x.Name);
    }

    [Fact]
    public void ApplyOrderBy_OrderByDescending_ReturnsSortedQuery()
    {
        var queryable = GetTestQueryable();
        var sortMap = GetSortMap();
        var request = new PaginatedRequest { OrderBy = "name", OrderByDescending = true };

        var result = queryable.Apply(sortMap, _map, request).ToList();

        result.Should().BeInDescendingOrder(x => x.Name);
    }

    [Fact]
    public void ApplyOrderBy_NoOrderBy_ReturnsSortedByIdDescending()
    {
        var queryable = GetTestQueryable();
        var sortMap = GetSortMap();
        var request = new PaginatedRequest { OrderBy = null };

        var result = queryable.Apply(sortMap, _map, request).ToList();

        result.Should().BeInDescendingOrder(x => x.Id);
    }

    [Fact]
    public void ApplyOrderBy_InvalidOrderByField_ThrowsArgumentException()
    {
        var queryable = GetTestQueryable();
        var sortMap = GetSortMap();
        var request = new PaginatedRequest { OrderBy = "invalidField" };

        void act() => queryable.Apply(sortMap, _map, request);

        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ApplyThenBy_ThenByAscending_ReturnsSortedQuery()
    {
        var queryable = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alice", Age = 30 },
            new() { Id = Guid.NewGuid(), Name = "Bob", Age = 20 },
            new() { Id = Guid.NewGuid(), Name = "Alice", Age = 25 }
        }.AsQueryable();
        var sortMap = GetSortMap();
        var request = new PaginatedRequest
        {
            OrderBy = "name",
            OrderByDescending = false,
            ThenBy = "age",
            ThenByDescending = false
        };

        var result = queryable.Apply(sortMap,_map, request).ToList();

        result.Should().ContainInOrder(
            queryable.Where(x => x.Name == "Alice").OrderBy(x => x.Age).First(),
            queryable.Where(x => x.Name == "Alice").OrderByDescending(x => x.Age).First(),
            queryable.Where(x => x.Name == "Bob").First()
        );
    }

    [Fact]
    public void ApplyThenBy_ThenByDescending_ReturnsSortedQuery()
    {
        var queryable = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alice", Age = 30 },
            new() { Id = Guid.NewGuid(), Name = "Bob", Age = 20 },
            new() { Id = Guid.NewGuid(), Name = "Alice", Age = 25 }
        }.AsQueryable();
        var sortMap = GetSortMap();
        var request = new PaginatedRequest
        {
            OrderBy = "name",
            OrderByDescending = false,
            ThenBy = "age",
            ThenByDescending = true
        };

        var result = queryable.Apply(sortMap,_map, request).ToList();

        result.Should().ContainInOrder(
            queryable.Where(x => x.Name == "Alice").OrderByDescending(x => x.Age).First(),
            queryable.Where(x => x.Name == "Alice").OrderBy(x => x.Age).First(),
            queryable.Where(x => x.Name == "Bob").First()
        );
    }

    [Fact]
    public void ApplyThenBy_NoThenBy_ReturnsCorrectlyOrderedQuery()
    {
        var queryable = GetTestQueryable();
        var sortMap = GetSortMap();
        var request = new PaginatedRequest
        {
            OrderBy = "name",
            OrderByDescending = false,
            ThenBy = null
        };

        var result = queryable.Apply(sortMap, _map, request).ToList();

        result.Should().BeInAscendingOrder(x => x.Name);
    }

    [Fact]
    public void ApplyPagination_ValidPageAndLimit_ReturnsPaginatedQuery()
    {
        var queryable = GetTestQueryable().OrderBy(x => x.Id); // Ensure consistent order for pagination
        var sortMap = GetSortMap(); // Sort map is not directly used by pagination, but needed for Apply method
        var request = new PaginatedRequest { Page = 2, Limit = 1 };

        var result = queryable.Apply(sortMap, _map, request).ToList();

        result.Should().HaveCount(1);
        result.First().Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000002"));
    }

    [Fact]
    public void ApplyPagination_PageNotInformed_ReturnsFirstPage()
    {
        var queryable = GetTestQueryable().OrderByDescending(x => x.Id);
        var sortMap = GetSortMap();
        var request = new PaginatedRequest { Limit = 2 };

        var result = queryable.Apply(sortMap, _map, request).ToList();

        result.Should().HaveCount(2);
        result.First().Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000003"));
    }

    [Fact]
    public void ApplyPagination_LimitNotInformed_UsesDefaultPaginationLimit()
    {
        var queryable = GetTestQueryable().OrderBy(x => x.Id);
        var sort = GetSortMap();
        var request = new PaginatedRequest { Page = 1 }; // Limit is 0, should use default 10

        var result = queryable.Apply(sort, _map, request).ToList();

        result.Should().HaveCount(3); // All 3 items should be returned if PaginationLimit is 10
    }

    [Fact]
    public void Filter_ApplyTrue_ReturnsFilteredQuery()
    {
        var queryable = GetTestQueryable();

        var result = queryable.Filter(true, x => x.Age > 25).ToList();

        result.Should().HaveCount(2);
    }

    [Fact]
    public void Filter_ApplyFalse_ReturnsUnfilteredQuery()
    {
        var queryable = GetTestQueryable();

        var result = queryable.Filter(false, x => x.Age > 25).ToList();

        result.Should().HaveCount(3); // All items should be returned
    }
}