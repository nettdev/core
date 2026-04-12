using Microsoft.EntityFrameworkCore.Diagnostics;
using Nett.Core.Domain;

namespace Nett.Core.Events;

public interface IEventDispatcher
{
    Task Dispatch(IEnumerable<IDomainEvent> events, CancellationToken cancellation = default);
}

public class EventDispatcher(IBus bus) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken ct = default)
    {
        if (eventData.Context is null)
            return result;

        var entries = eventData.Context.ChangeTracker.Entries<Entity>();
        var entitites = entries.Select(entry => entry.Entity).ToList();
        var events = entitites.SelectMany(entity => entity.Events).ToList();

        entitites.ForEach(entity => entity.ClearEvents());

        await bus.Publish(events, ct);

        return result;
    }
}