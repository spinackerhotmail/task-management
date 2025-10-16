using TaskManagementService.CommonLib.Domain;
using TaskManagementService.Domain.Entities;

namespace TaskManagementService.Domain.Events;

public class TaskEntityUpdated : AbstractBaseEvent<TaskEntity>
{
    public TaskEntityUpdated(TaskEntity item) : base(item) { }
}
