using Nett.Core.Events;

namespace Nett.Core.Domain;

[ExcludeFromCodeCoverage]
public abstract class Entity : IEquatable<Entity>
{
    private readonly List<IDomainEvent> _events = [];
    public Guid Id { get; init; }
    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    protected Entity()
    {
        Id = Guid.CreateVersion7();
    }

    protected void AddEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }

    public static bool operator ==(Entity a, Entity b) =>
        a.Equals(b);

    public static bool operator !=(Entity a, Entity b) =>
        !(a == b);

    public override int GetHashCode() =>
        (GetType().GetHashCode() ^ 93) + Id.GetHashCode();

    public bool Equals(Entity? other) =>
        Id == other?.Id;

    public override bool Equals(object? obj) =>
        obj is Entity entity && Id == entity.Id;
}