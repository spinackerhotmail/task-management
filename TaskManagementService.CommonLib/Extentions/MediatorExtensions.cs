using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementService.CommonLib.Domain;

namespace TaskManagementService.CommonLib.Extentions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context)
    {
        // Collect all tracked entities that have domain events (supporting generic base types)
        var domainEventEntities = context.ChangeTracker
            .Entries()
            .Where(e => e.Entity is IHasDomainEvents hasEvents && hasEvents.DomainEvents.Any())
            .Select(e => (IHasDomainEvents)e.Entity)
            .ToList();

        var domainEvents = domainEventEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEventEntities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
