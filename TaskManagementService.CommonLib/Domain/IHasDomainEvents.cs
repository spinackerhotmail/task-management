using System.Diagnostics.Tracing;

namespace TaskManagementService.CommonLib.Domain;

public interface IHasDomainEvents
{
    IReadOnlyCollection<EventBase> DomainEvents { get; }
    void ClearDomainEvents();
}
