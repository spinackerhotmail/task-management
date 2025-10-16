using TaskManagementService.CommonLib.Domain;
using TaskManagementService.Domain.Entities;

namespace TaskManagementService.Domain.Events;

public class TaskEntityCreated : AbstractBaseEvent<TaskEntity>
{
    public TaskEntityCreated(TaskEntity item) : base(item) { }
}
