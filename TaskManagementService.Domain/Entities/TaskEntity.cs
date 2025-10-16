using TaskManagementService.CommonLib.Audits;
using TaskManagementService.CommonLib.Domain;
using TaskManagementService.Domain.Enums;

namespace TaskManagementService.Domain.Entities;

public class TaskEntity : AuditableEntityBase<Guid>, IAuditable
{
    public required string UserId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public TaskEntityStatus Status { get; set; } = TaskEntityStatus.New;
}
