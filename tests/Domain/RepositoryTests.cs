using System.Linq.Expressions;
using Nett.Core.Models;
using Nett.Core.Domain;
using Microsoft.EntityFrameworkCore.Query;
using Shouldly;
using Nett.Core.Persistence;

namespace Nett.Core.UnitTest.Domain;

[ExcludeFromCodeCoverage]
public class TestAggregateRoot : AggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

[ExcludeFromCodeCoverage]
public class TestPagedRequest : PagedRequest<TestAggregateRoot, string>
{
    public int? Age { get; set; }

    public override IReadOnlyCollection<Expression<Func<TestAggregateRoot, bool>>> ToFilters()
    {
        var filters = new List<Expression<Func<TestAggregateRoot, bool>>>();

        if (Age is {} age)
            filters.Add(x => x.Age > age);

        return filters;
    }

    public override Expression<Func<TestAggregateRoot, string>> ToProjection()
    {
        return x => x.Name;
    }
}

public class TestRepository(IQueryable<TestAggregateRoot> queriable) : Repository<TestAggregateRoot>
{
    protected override IQueryable<TestAggregateRoot> Queryable { get; } = queriable;
    protected override Dictionary<string, Expression<Func<TestAggregateRoot, object>>> SortMap { get; } = new()
    {
        ["name"] = x => x.Name,
        ["age"] = x => x.Age
    };
}

public class RepositoryTests
{
    private readonly TestRepository _repository;
    private readonly IQueryable<TestAggregateRoot> _queryable;

    public RepositoryTests()
    {
        _queryable = CreateTestQueryable();
        _repository = new TestRepository(_queryable);
    }

    [Fact]
    public async Task Query_WhenReceivePagedRequest_ShouldReturnPagedResponse()
    {
        var request = new TestPagedRequest { Page = 1, Limit = 2 };
        var result = await _repository.Query(request, CancellationToken.None);

        Assert.Equal(2, result.Items.Count());
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(2, result.Limit);
    }

    [Fact]
    public async Task Query_WhenReceiveFilteredRequest_ShouldApplyFilters()
    {
        var request = new TestPagedRequest { Page = 1, Limit = 10, Age = 30 };
        var result = await _repository.Query(request, CancellationToken.None);

        result.Items.ShouldHaveSingleItem();
        result.TotalCount.ShouldBeEquivalentTo(1);
    }

    [Fact]
    public async Task Query_WhenDontReceiveSortField_ShouldApplyFirstSorting()
    {
        var request = new TestPagedRequest { Page = 1, Limit = 10 };
        var result = await _repository.Query(request, CancellationToken.None);

        Assert.Equal("Alice", result.Items.First());
        Assert.Equal("Charlie", result.Items.Last());
    }

    [Fact]
    public async Task Query_WhenReceiveSortField_ShouldApplyInformedSorting()
    {
        var request = new TestPagedRequest { Page = 1, Limit = 10, OrderBy = "age" };
        var result = await _repository.Query(request, CancellationToken.None);

        Assert.Equal("Bob", result.Items.First());
        Assert.Equal("Charlie", result.Items.Last());
    }

    [Fact]
    public async Task Query_WhenReceiveDescendingSortField_ShouldApplyDescendingSorting()
    {
        var request = new TestPagedRequest { Page = 1, Limit = 10, OrderBy = "name", OrderByDescending = true };
        var result = await _repository.Query(request, CancellationToken.None);

        Assert.Equal("Charlie", result.Items.First());
        Assert.Equal("Alice", result.Items.Last());
    }

    [Fact]
    public async Task Query_WhenReceiveSortFields_ShouldApplySortingFields()
    {
        var request = new TestPagedRequest { Page = 1, Limit = 10, OrderBy = "name", ThenBy = "age" };
        var result = await _repository.Query(request, CancellationToken.None);

        Assert.Equal("Alice", result.Items.First());
        Assert.Equal("Charlie", result.Items.Last());
    }

    [Fact]
    public async Task Query_WhenReceiveRequest_ShouldApplyPagination()
    {
        var request = new TestPagedRequest { Page = 2, Limit = 1 };
        var result = await _repository.Query(request, CancellationToken.None);

        Assert.Single(result.Items);
        Assert.Equal("Bob", result.Items.First());
        Assert.Equal(3, result.TotalCount);
    }

    [Fact]
    public async Task GetSort_ShouldThrowExceptionForInvalidField()
    {
        await Assert.ThrowsAsync<InvalidSortFieldException>(async () => 
        {
            var request = new TestPagedRequest { Page = 1, Limit = 10, OrderBy = "invalid" };
            Expression<Func<TestAggregateRoot, string>> map = x => x.Name;
            _ = await _repository.Query(request, CancellationToken.None);
        });
    }

    private static TestAsyncEnumerable<TestAggregateRoot> CreateTestQueryable()
    {
        var data = new List<TestAggregateRoot>
        {
            new() { Id = Guid.NewGuid(), Name = "Alice", Age = 30 },
            new() { Id = Guid.NewGuid(), Name = "Bob", Age = 25 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Age = 35 }
        }.AsQueryable();

        return new TestAsyncEnumerable<TestAggregateRoot>(data);
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
    public TestAsyncEnumerable(Expression expression) : base(expression) { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

internal class TestAsyncEnumerator<T>(IEnumerator<T> inner) : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner = inner;

    public T Current => _inner.Current;

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(_inner.MoveNext());
}

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

    public IQueryable CreateQuery(Expression expression) =>
        new TestAsyncEnumerable<TEntity>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
        new TestAsyncEnumerable<TElement>(expression);

    public object Execute(Expression expression) => _inner.Execute(expression)!;

    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        var resultType = typeof(TResult).GetGenericArguments()[0];
        var executionResult = typeof(IQueryProvider)
            .GetMethod(name: "Execute", genericParameterCount: 1, types: [typeof(Expression)])!
            .MakeGenericMethod(resultType)
            .Invoke(this, [expression])!;

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(resultType)!
            .Invoke(null, [executionResult])!;
    }
}
