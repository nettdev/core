using Nett.Core.Domain;
using Nett.Core.Events;
using Shouldly;

namespace Nett.Core.UnitTest.Events;

[ExcludeFromCodeCoverage]
public class EntityEventsTests
{
    [Fact]
    public void AddEvent_SingleEvent_AppearsInEventsCollection()
    {
        // Arrange
        var entity = new TestEntity();
        var domainEvent = new TestDomainEvent();

        // Act
        entity.RaiseEvent(domainEvent);

        // Assert
        entity.Events.ShouldHaveSingleItem();
        entity.Events.ShouldContain(domainEvent);
    }

    [Fact]
    public void AddEvent_MultipleEvents_AllAppearInCollection()
    {
        // Arrange
        var entity = new TestEntity();
        var event1 = new TestDomainEvent();
        var event2 = new TestDomainEvent();

        // Act
        entity.RaiseEvent(event1);
        entity.RaiseEvent(event2);

        // Assert
        entity.Events.Count.ShouldBe(2);
        entity.Events.ShouldContain(event1);
        entity.Events.ShouldContain(event2);
    }

    [Fact]
    public void ClearEvents_WithExistingEvents_EmptiesCollection()
    {
        // Arrange
        var entity = new TestEntity();
        entity.RaiseEvent(new TestDomainEvent());
        entity.RaiseEvent(new TestDomainEvent());

        // Act
        entity.ClearEvents();

        // Assert
        entity.Events.ShouldBeEmpty();
    }

    [Fact]
    public void ClearEvents_WhenEmpty_RemainsEmpty()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.ClearEvents();

        // Assert
        entity.Events.ShouldBeEmpty();
    }

    [Fact]
    public void Events_NewEntity_IsEmpty()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.Events.ShouldBeEmpty();
    }
}

[ExcludeFromCodeCoverage]
file sealed class TestEntity : Entity
{
    public void RaiseEvent(IDomainEvent @event) => AddEvent(@event);
}

[ExcludeFromCodeCoverage]
file sealed record TestDomainEvent : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
    public string Name => nameof(TestDomainEvent);
}
