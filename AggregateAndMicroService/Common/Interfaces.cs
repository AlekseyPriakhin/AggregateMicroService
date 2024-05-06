using System.Collections.ObjectModel;

using MediatR;

namespace AggregateAndMicroService.Common;


// Aggregates Interfaces
public interface IAggregate<T> : IAggregate, IEntity<T>
{
}

public interface IAggregate : IEntity
{
}

// Entity
public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}

public interface IEntity
{
    public bool IsDeleted { get; set; }
}


public interface IDomainEventGenerator
{
    public IReadOnlyCollection<INotification> DomainEvents { get; }

    public void AddDomainEvent(INotification eventItem);

    public void RemoveDomainEvent(INotification eventItem);

    public void ClearDomainEvents();
}
