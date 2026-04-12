namespace Nett.Core.Events;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
    public string Name { get; }
};

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent domainEvent, CancellationToken cancellation = default);
}
