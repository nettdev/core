namespace Nett.Core.Events;

public interface IDomainEvent;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(T @event, CancellationToken cancellation = default);
}
