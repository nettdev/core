using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nett.Core.Events;

public interface IBus
{
    Task Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellation = default) where TEvent : IDomainEvent;
}

public partial class InMemoryBus(IServiceProvider serviceProvider, ILogger<InMemoryBus> logger) : IBus
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Publishing event {EventName}. Content {@Content}")]
    public static partial void LogEventPublished(ILogger logger, string eventName, object content);

    public async Task Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellation = default) where TEvent : IDomainEvent
    {
        foreach (var domainEvent in events)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = serviceProvider.GetServices(handlerType);

            LogEventPublished(logger, domainEvent.Name, domainEvent);

            foreach (var handler in handlers)
            {
                var method = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.Handle))!;
                await (Task)method.Invoke(handler, [domainEvent, cancellation])!;
            }
        }
    }
}
