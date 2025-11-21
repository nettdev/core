namespace Nett.Core.Events;

public interface IBus
{
    Task Publish<T>(T @event, CancellationToken cancellation = default) where T : IDomainEvent;
}
