# Nett.Core

[![codecov](https://codecov.io/github/nettdev/core/graph/badge.svg?token=tpEkNY1NwN)](https://codecov.io/github/nettdev/core)
[![Nuget](https://img.shields.io/nuget/v/Nett.Core.svg)](https://www.nuget.org/packages/Nett.Core)
[![License](https://img.shields.io/github/license/nettdev/core.svg)](https://github.com/nettdev/core/blob/main/LICENSE)

A lightweight .NET library providing foundational building blocks for **Clean Architecture** and **Domain-Driven Design (DDD)** in .NET 10+ applications.

## 🚀 Features

- **Entity & AggregateRoot**: Base classes for domain entities with built-in domain event dispatching support.
- **Enumeration**: Type-safe, value-based enumerations.
- **Repository Pattern**: Repository base with support for **paging**, **sorting**, **filtering**, and **projections**.
- **Specifications**: Composite specification pattern for encapsulating query logic.
- **Result & Error Handling**: Functional-style `Result<T>` monad for robust error handling without exceptions.
- **Paged Queries**: `PagedRequest` and `PagedResponse` for efficient data retrieval.
- **Fluent Validations**: `ParameterRuleBuilder` for parameter and model validation.
- **Domain Events**: Simple event sourcing with `IDomainEvent` and dispatcher integration for EF Core.

Framework-agnostic core, with seamless integration for **ASP.NET Core Minimal APIs** and **Entity Framework Core**.

## 📦 Installation

Add the package to your project:

```bash
dotnet add package Nett.Core
```

## 🚀 Quick Start

### 1. Domain Entities & Aggregates

Inherit from `Entity` or `AggregateRoot`:

```csharp
using Nett.Core.Domain;
using Nett.Core.Events;

public record PostCreated(string Title, DateTimeOffset CreatedAt) : IDomainEvent;

public class Post : AggregateRoot
{
    public string Title { get; init; } = default!;
    public string Content { get; init; } = default!;
    public DateTimeOffset CreatedAt { get; init; } = DateTime.UtcNow;

    public Post(string title, string content)
    {
        Title = title;
        Content = content;
        AddEvent(new PostCreated(Title, CreatedAt));
    }
}
```

### 2. Base Repository

Implement your repository by overriding `Queryable` and `SortMap`:

```csharp
using Microsoft.EntityFrameworkCore;
using Nett.Core.Domain;
using Nett.Core.Persistence;

public class PostRepository(AppDbContext db) : Repository<Post>, IPostRepository
{
    protected override IQueryable<Post> Queryable => db.Posts;

    protected override Dictionary<string, Expression<Func<Post, object>>> SortMap => new()
    {
        [nameof(Post.Title)] = p => p.Title,
        [nameof(Post.CreatedAt)] = p => p.CreatedAt,
    };
}
```

### 3. Paginated Queries with Minimal APIs

Define a paged request:

```csharp
using Nett.Core.Models;
using Microsoft.EntityFrameworkCore;

public record PostResponse(string Title, string Content);

public sealed class PostRequest : PagedRequest<Post, PostResponse>
{
    public string? Search { get; set; }

    public override IReadOnlyCollection<Expression<Func<Post, bool>>> ToFilters()
    {
        var filters = new List<Expression<Func<Post, bool>>>();
        if (!string.IsNullOrWhiteSpace(Search))
        {
            filters.Add(x => EF.Functions.ILike(x.Title, $"%{Search}%") ||
                             EF.Functions.ILike(x.Content, $"%{Search}%"));
        }
        return filters;
    }

    public override Expression<Func<Post, PostResponse>> ToProjection() =>
        x => new PostResponse(x.Title, x.Content);
}
```

Use in endpoint:

```csharp
app.MapGet("/", async (PostRepository repo, [AsParameters] PostRequest request, CancellationToken ct) =>
{
    var response = await repo.Query(request, ct);
    return response;
});
```

Supports `?page=1&limit=10&orderBy=Title&orderByDescending=true&search=GPT&thenBy=CreatedAt`.

### 4. Domain Events with EF Core

```csharp
public class AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventsDispatcher dispatcher)
    : DbContext(options)
{
    public DbSet<Post> Posts { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        await dispatcher.DispatchEventsAsync(this, ct);
        return await base.SaveChangesAsync(ct);
    }
}
```

Register:

```csharp
builder.Services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
builder.Services.AddScoped<IPostRepository, IPostRepository>();
```

### 5. Result & Error Handling

```csharp
var result = Result.Success(new Post());
if (result.IsFailure)
{
    return result.Error;
}

var result = ParameterRuleBuilder
    .RuleFor(() => title)
    .NotEmpty()
    .MinLength(5)
    .Build(() => new Post(title));

result.Match(
    post => Console.WriteLine("Valid post {}", post.Title), 
    error => Console.WriteLine("Validation error {}", error.Errors.Message)
)

if (result.IsFailure)
{
    return validated.Error;
}

var post = result.Value;
```

### 6. Fluent Parameter Validation

```csharp
public static Result<Person> Create(string email, int age)
{
    return ParameterRuleBuilder
        .RuleFor(() => email).Email().NotEmpty()
        .RuleFor(() => age).GreaterThan(18)
        .Build(() => new Person(email, age));
}
```

## 📚 Examples

See the [examples](examples/) folder for a full Minimal API demo with PostgreSQL, EF Core, and domain events.

## 🧪 Testing

Comprehensive unit tests in `/tests`. Run with:

```bash
dotnet test
```

## 📄 License

MIT License - see [LICENSE](LICENSE)

## 🙏 Acknowledgments

Built with ❤️ for .NET developers embracing Clean Architecture and DDD.

[GitHub Repo](https://github.com/nettdev/core) | [NuGet](https://www.nuget.org/packages/Nett.Core)
