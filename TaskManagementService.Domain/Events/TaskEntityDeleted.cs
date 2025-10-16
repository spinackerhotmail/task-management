using TaskManagementService.CommonLib.Domain;
using TaskManagementService.Domain.Entities;

namespace TaskManagementService.Domain.Events;

public class TaskEntityDeleted : AbstractBaseEvent<TaskEntity>
{
    public TaskEntityDeleted(TaskEntity item) : base(item) { }
}
