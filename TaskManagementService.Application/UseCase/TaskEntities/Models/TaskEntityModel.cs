using TaskManagementService.Domain.Enums;

namespace TaskManagementService.Application.UseCase.TaskEntities.Models
{
    public class TaskEntityModel
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = null!;
        public TaskEntityStatus Status { get; set; } = TaskEntityStatus.New;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}

