using Microsoft.Extensions.DependencyInjection;
using Nett.Core.Events;
using NSubstitute;
using Shouldly;

namespace Nett.Core.UnitTest.Events;

[ExcludeFromCodeCoverage]
public class InMemoryBusTests
{
    [Fact]
    public async Task Publish_SingleEvent_InvokesRegisteredHandler()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var handler = Substitute.For<IDomainEventHandler<OrderPlacedEvent>>();
        var services = new ServiceCollection();
        services.AddSingleton(handler);
        var bus = new InMemoryBus(services.BuildServiceProvider());

        var domainEvent = new OrderPlacedEvent();

        // Act
        await bus.Publish([domainEvent], ct);

        // Assert
        await handler.Received(1).Handle(domainEvent, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Publish_MultipleEvents_InvokesHandlerForEach()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var handler = Substitute.For<IDomainEventHandler<OrderPlacedEvent>>();
        var services = new ServiceCollection();
        services.AddSingleton(handler);
        var bus = new InMemoryBus(services.BuildServiceProvider());

        var event1 = new OrderPlacedEvent();
        var event2 = new OrderPlacedEvent();

        // Act
        await bus.Publish([event1, event2], ct);

        // Assert
        await handler.Received(2).Handle(Arg.Any<OrderPlacedEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Publish_MultipleHandlers_AllHandlersInvoked()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var handler1 = Substitute.For<IDomainEventHandler<OrderPlacedEvent>>();
        var handler2 = Substitute.For<IDomainEventHandler<OrderPlacedEvent>>();
        var services = new ServiceCollection();
        services.AddSingleton(handler1);
        services.AddSingleton(handler2);
        var bus = new InMemoryBus(services.BuildServiceProvider());

        var domainEvent = new OrderPlacedEvent();

        // Act
        await bus.Publish([domainEvent], ct);

        // Assert
        await handler1.Received(1).Handle(domainEvent, Arg.Any<CancellationToken>());
        await handler2.Received(1).Handle(domainEvent, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Publish_NoHandlersRegistered_CompletesWithoutError()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var bus = new InMemoryBus(new ServiceCollection().BuildServiceProvider());
        var domainEvent = new OrderPlacedEvent();

        // Act & Assert
        await Should.NotThrowAsync(() => bus.Publish([domainEvent], ct));
    }

    [Fact]
    public async Task Publish_EmptyEventList_CompletesWithoutInvokingHandlers()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var handler = Substitute.For<IDomainEventHandler<OrderPlacedEvent>>();
        var services = new ServiceCollection();
        services.AddSingleton(handler);
        var bus = new InMemoryBus(services.BuildServiceProvider());

        // Act
        await bus.Publish(Array.Empty<OrderPlacedEvent>(), ct);

        // Assert
        await handler.DidNotReceive().Handle(Arg.Any<OrderPlacedEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Publish_PassesCancellationToken_ToHandler()
    {
        // Arrange
        var handler = Substitute.For<IDomainEventHandler<OrderPlacedEvent>>();
        var services = new ServiceCollection();
        services.AddSingleton(handler);
        var bus = new InMemoryBus(services.BuildServiceProvider());
        var cancellation = TestContext.Current.CancellationToken;

        var domainEvent = new OrderPlacedEvent();

        // Act
        await bus.Publish([domainEvent], cancellation);

        // Assert
        await handler.Received(1).Handle(domainEvent, cancellation);
    }
}

[ExcludeFromCodeCoverage]
internal sealed record OrderPlacedEvent : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
    public string Name => nameof(OrderPlacedEvent);
}
