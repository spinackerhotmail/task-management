namespace TaskManagementService.CommonLib.Domain;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
    string? CreatedByUser { get; set; }
    string? UpdatedByUser { get; set; }
}
