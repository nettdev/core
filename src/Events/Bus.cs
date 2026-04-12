using Microsoft.Extensions.DependencyInjection;

namespace Nett.Core.Events;

public interface IBus
{
    Task Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellation = default) where TEvent : IDomainEvent;
}

public sealed class InMemoryBus(IServiceProvider serviceProvider) : IBus
{
    public async Task Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellation = default) where TEvent : IDomainEvent
    {
        foreach (var domainEvent in events)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var method = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.Handle))!;
                await (Task)method.Invoke(handler, [domainEvent, cancellation])!;
            }
        }
    }
}
