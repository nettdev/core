using Microsoft.EntityFrameworkCore;
using Nett.Core.Domain;
using Nett.Core.Events;
using NSubstitute;
using Shouldly;

namespace Nett.Core.UnitTest.Events;

[ExcludeFromCodeCoverage]
public class EventDispatcherTests
{
    [Fact]
    public async Task SaveChangesAsync_EntityWithEvent_PublishesEvent()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var bus = Substitute.For<IBus>();
        await using var context = BuildContext(bus);

        var domainEvent = new ShipmentDispatchedEvent();
        var entity = new DispatchableEntity();
        entity.Dispatch(domainEvent);
        context.Set<DispatchableEntity>().Add(entity);

        // Act
        await context.SaveChangesAsync(ct);

        // Assert
        await bus.Received(1).Publish(
            Arg.Is<IEnumerable<IDomainEvent>>(e => e.Contains(domainEvent)),
            ct);
    }

    [Fact]
    public async Task SaveChangesAsync_EntityWithEvent_ClearsEventsAfterPublish()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var bus = Substitute.For<IBus>();
        await using var context = BuildContext(bus);

        var entity = new DispatchableEntity();
        entity.Dispatch(new ShipmentDispatchedEvent());
        context.Set<DispatchableEntity>().Add(entity);

        // Act
        await context.SaveChangesAsync(ct);

        // Assert
        entity.Events.ShouldBeEmpty();
    }

    [Fact]
    public async Task SaveChangesAsync_MultipleEntitiesWithEvents_PublishesAllEvents()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var bus = Substitute.For<IBus>();
        await using var context = BuildContext(bus);

        var event1 = new ShipmentDispatchedEvent();
        var event2 = new ShipmentDispatchedEvent();

        var entity1 = new DispatchableEntity();
        entity1.Dispatch(event1);

        var entity2 = new DispatchableEntity();
        entity2.Dispatch(event2);

        context.Set<DispatchableEntity>().AddRange(entity1, entity2);

        // Act
        await context.SaveChangesAsync(ct);

        // Assert
        await bus.Received(1).Publish(
            Arg.Is<IEnumerable<IDomainEvent>>(e => e.Contains(event1) && e.Contains(event2)),
            ct);
    }

    [Fact]
    public async Task SaveChangesAsync_EntityWithNoEvents_PublishesEmptyList()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var bus = Substitute.For<IBus>();
        await using var context = BuildContext(bus);

        context.Set<DispatchableEntity>().Add(new DispatchableEntity());

        // Act
        await context.SaveChangesAsync(ct);

        // Assert
        await bus.Received(1).Publish(
            Arg.Is<IEnumerable<IDomainEvent>>(e => !e.Any()),
            ct);
    }

    [Fact]
    public async Task SaveChangesAsync_EntityWithMultipleEvents_PublishesAllAndClearsAll()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var bus = Substitute.For<IBus>();
        await using var context = BuildContext(bus);

        var event1 = new ShipmentDispatchedEvent();
        var event2 = new ShipmentDispatchedEvent();

        var entity = new DispatchableEntity();
        entity.Dispatch(event1);
        entity.Dispatch(event2);
        context.Set<DispatchableEntity>().Add(entity);

        // Act
        await context.SaveChangesAsync(ct);

        // Assert
        await bus.Received(1).Publish(
            Arg.Is<IEnumerable<IDomainEvent>>(e => e.Contains(event1) && e.Contains(event2)),
            ct);
        entity.Events.ShouldBeEmpty();
    }

    private static DbContext BuildContext(IBus bus) =>
        DispatchableDbContext.Create(bus);
}

[ExcludeFromCodeCoverage]
file sealed record ShipmentDispatchedEvent : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
    public string Name => nameof(ShipmentDispatchedEvent);
}

[ExcludeFromCodeCoverage]
file sealed class DispatchableEntity : Entity
{
    public void Dispatch(IDomainEvent @event) => AddEvent(@event);
}

[ExcludeFromCodeCoverage]
file sealed class DispatchableDbContext(DbContextOptions<DispatchableDbContext> options) : DbContext(options)
{
    public DbSet<DispatchableEntity> Entities => Set<DispatchableEntity>();

    public static DispatchableDbContext Create(IBus bus)
    {
        var options = new DbContextOptionsBuilder<DispatchableDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddInterceptors(new EventDispatcher(bus))
            .Options;

        return new DispatchableDbContext(options);
    }
}
