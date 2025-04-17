namespace Nett.Core.Domain;

[ExcludeFromCodeCoverage]
public class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected set; }

    protected Entity() 
    {
        Id = Guid.NewGuid();
    }

    public static bool operator == (Entity a, Entity b) =>
        a.Equals(b);

    public static bool operator != (Entity a, Entity b) =>
        !(a == b);

    public override int GetHashCode() =>
        (GetType().GetHashCode() ^ 93) + Id.GetHashCode();

    public bool Equals(Entity? other) =>
        Id == other?.Id;

    public override bool Equals(object? obj) =>
        obj is Entity entity && Id == entity.Id;
}