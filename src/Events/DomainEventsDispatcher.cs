using Microsoft.EntityFrameworkCore;
using Nett.Core.Domain;

namespace Nett.Core.Events;

public interface IDomainEventsDispatcher
{
    Task Dispatch(DbContext dbContext, CancellationToken cancellation = default);
}

public sealed class DomainEventsDispatcher(IBus bus) : IDomainEventsDispatcher
{
    private readonly IBus _bus = bus;

    public async Task Dispatch(DbContext dbContext, CancellationToken cancellation = default)
    {
        var entitiesWithEvents = dbContext.ChangeTracker.Entries<AggregateRoot>().Where(e => e.Entity.Events.Count > 0).ToList();
        var domainEvents = entitiesWithEvents.SelectMany(e => e.Entity.Events).ToList();

        entitiesWithEvents.ForEach(e => e.Entity.ClearEvents());

        foreach (var @event in domainEvents)
        {
            await _bus.Publish(@event, cancellation);
        }
    }
}
