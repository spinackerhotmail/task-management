using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementService.CommonLib.Domain;

public class EntityBase<TId> : IHasDomainEvents
    where TId : notnull
{
    private readonly List<EventBase> domainEvents = new();

    public TId Id { get; set; } = default!;
    public string? ExternalId { get; set; }

    [NotMapped]
    public IReadOnlyCollection<EventBase> DomainEvents => domainEvents.AsReadOnly();

    public void AddDomainEvent(EventBase domainEvent) => domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(EventBase domainEvent) => domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => domainEvents.Clear();

    public async Task DispatchEvents(IMediator mediator)
    {
        foreach (var domainEvent in DomainEvents)
            await mediator.Publish(domainEvent);

        ClearDomainEvents();
    }
}

public abstract class EntityBase : EntityBase<long>
{ }
