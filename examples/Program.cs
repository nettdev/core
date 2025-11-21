using System.Linq.Expressions;
using System.Text.Json;
using Main;
using Microsoft.EntityFrameworkCore;
using Nett.Core.Domain;
using Nett.Core.Events;
using Nett.Core.Models;
using Nett.Core.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<InvoiceRepository>(); 
builder.Services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

WebApplication app = builder.Build();

app.MapGet("/", async (InvoiceRepository repository, [AsParameters] PostRequest request, CancellationToken cancellation) =>
{
    var results = await repository.Query(request, cancellation);
    return results; 
});

app.Map("/save", async (AppDbContext db) =>
{
    var post = new Post("GPT 5.1 Lançado", "A nova versão do modelo GPT traz melhorias significativas em compreensão de contexto");
    await db.Posts.AddAsync(post);
    await db.SaveChangesAsync();
    return true;
});

await app.RunAsync();

namespace Main
{
    public record PostResponse(string Title, string Content);

    public sealed class PostRequest : PagedRequest<Post, PostResponse>
    {
        public string? Search { get; set; }

        public override IReadOnlyCollection<Expression<Func<Post, bool>>> ToFilters()
        {
            var expressions = new List<Expression<Func<Post, bool>>>();

            if (!string.IsNullOrWhiteSpace(Search))
                expressions.Add(x => EF.Functions.ILike(x.Title, $"%{Search}%") || EF.Functions.ILike(x.Content, $"%{Search}%"));

            return expressions;
        }

        public override Expression<Func<Post, PostResponse>> ToProjection()
        {
            return x => new PostResponse(x.Title, x.Content);
        }
    }

    public class AppDbContext : DbContext
    {
        private readonly IDomainEventsDispatcher _eventsDispatcher;

        public DbSet<Post> Posts => Set<Post>();

        public AppDbContext(IDomainEventsDispatcher eventsDispatcher)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            SeedData();
            _eventsDispatcher = eventsDispatcher;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.UseNpgsql("Host=localhost; Database=sandbox; Username=postgres; Password=postgres");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _eventsDispatcher.Dispatch(this, cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SeedData()
        {
            if (!Posts.Any())
            {
                var posts = new List<Post>
                {
                    new("GPT 5.1 Lançado", "A nova versão do modelo GPT traz melhorias significativas em compreensão de contexto, geração de texto mais coerente e suporte a múltiplos idiomas."),
                    new("Gemini 3.0 Pro Lançado", "Gemini 3.0 Pro apresenta arquitetura de última geração, oferecendo velocidade de inferência aumentada e menor consumo de memória."),
                    new("MiniMax M2 Lançado", "Perfeito para soluções embarcadas que requerem inteligência artificial local sem comprometer a eficiência energética.")
                };

                Posts.AddRange(posts);
                SaveChanges();
            }
        }
    }

    public record PostCreated(string Title, DateTimeOffset CreatedAt) : IDomainEvent;

    public sealed class PostCreatedHandler : IDomainEventHandler<PostCreated>
    {
        public async Task Handle(PostCreated @event, CancellationToken cancellation = default)
        {
            await File.WriteAllTextAsync("post-created.json", JsonSerializer.Serialize(@event), cancellation);
        }

        public async Task Handle(IDomainEvent @event, CancellationToken cancellation = default)
        {
            if (@event is PostCreated postCreated)
            {
                await Handle(postCreated, cancellation);
            }
        }
    }

    public class Post : AggregateRoot
    {
        public string Title { get; init; }
        public string Content { get; init; }
        public DateTimeOffset CreatedAt { get; init; } = DateTime.UtcNow;

        public Post(string title, string content)
        {
            Title = title;
            Content = content;
            AddEvent(new PostCreated(title, CreatedAt));
        }
    }

    public class InvoiceRepository(AppDbContext db) : Repository<Post>
    {
        protected override IQueryable<Post> Queryable => db.Posts;

        protected override Dictionary<string, Expression<Func<Post, object>>> SortMap => new() {
            [nameof(Post.Title)] = p => p.Title,
            [nameof(Post.CreatedAt)] = p => p.CreatedAt,
        };
    }
}
