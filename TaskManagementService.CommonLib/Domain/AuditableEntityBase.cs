namespace TaskManagementService.CommonLib.Domain;

public abstract class AuditableEntityBase<TId> : EntityBase<TId>, IAuditableEntity
    where TId : notnull
{
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата изменения
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Пользователь, создавший сущность
    /// </summary>
    public string? CreatedByUser { get; set; }

    /// <summary>
    /// Пользователь, изменивший сущность
    /// </summary>
    public string? UpdatedByUser { get; set; }
}

public abstract class AuditableEntityBase : AuditableEntityBase<long>
{ }
