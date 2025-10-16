using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskManagementService.CommonLib.Audits;
using TaskManagementService.CommonLib.Configurations;
using TaskManagementService.CommonLib.Extentions;

namespace TaskManagementService.CommonLib.DbContexts;

public class AuditableDbContext(DbContextOptions options, IMediator mediator) : DbContext(options)
{
    private readonly IMediator mediator = mediator;

    public DbSet<Audit> Audits => Set<Audit>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AuditConfiguration).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEvents(this);

        var auditEntries = OnBeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);
        await OnAfterSaveChanges(auditEntries);
        return result;
    }

    private List<AuditEntry> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var entries = new List<AuditEntry>();

        foreach (var entry in ChangeTracker.Entries())
        {
            // Do not audit entities that are not tracked, not changed, or not of type IAuditable
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged || entry.Entity is not IAuditable)
                continue;

            var auditEntry = new AuditEntry()
            {
                Audit = new Audit
                {
                    ActionType = entry.State == EntityState.Added ? "INSERT" : entry.State == EntityState.Deleted ? "DELETE" : "UPDATE",
                    EntityId = entry.Properties.Single(p => p.Metadata.IsPrimaryKey()).CurrentValue.ToString(),
                    EntityName = entry.Metadata.ClrType.Name,
                    TimeStamp = DateTime.UtcNow,
                    Changes = entry.Properties.Select(p => new { p.Metadata.Name, p.CurrentValue }).ToDictionary(i => i.Name, i => i.CurrentValue),
                },

                // TempProperties are properties that are only generated on save, e.g. ID's
                // These properties will be set correctly after the audited entity has been saved
                TempProperties = entry.Properties.Where(p => p.IsTemporary).ToList(),
            };

            entries.Add(auditEntry);
        }

        return entries;
    }

    private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
    {
        if (auditEntries == null || auditEntries.Count == 0)
            return Task.CompletedTask;

        // For each temporary property in each audit entry - update the value in the audit entry to the actual (generated) value
        foreach (var entry in auditEntries)
        {
            foreach (var prop in entry.TempProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    entry.Audit.EntityId = prop.CurrentValue.ToString();
                    entry.Audit.Changes[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    entry.Audit.Changes[prop.Metadata.Name] = prop.CurrentValue;
                }
            }
        }

        Audits.AddRange(auditEntries.Select(x => x.Audit));
        return SaveChangesAsync();
    }

}
