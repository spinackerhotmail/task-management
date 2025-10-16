using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using TaskManagementService.CommonLib.Domain;
using TaskManagementService.CommonLib.Services;

namespace TaskManagementService.CommonLib.Interceprots;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IDateTime dateTime;

    public AuditableEntitySaveChangesInterceptor(IDateTime dateTime)
    {
        this.dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is not IAuditableEntity auditable)
                continue;

            if (entry.State == EntityState.Added)
            {
                auditable.CreatedAt = dateTime.Now;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                auditable.UpdatedAt = dateTime.Now;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}