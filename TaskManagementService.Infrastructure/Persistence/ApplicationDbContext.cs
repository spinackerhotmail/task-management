using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.CommonLib.DbContexts;
using TaskManagementService.CommonLib.Interceprots;
using TaskManagementService.Domain.Entities;

namespace TaskManagementService.Infrastructure.Persistence;

public class ApplicationDbContext : AuditableDbContext, IApplicationDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor;

    public DbSet<TaskEntity> TaskEntities => Set<TaskEntity>();

    public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IMediator mediator, 
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options, mediator)
    { 
        this.auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditableEntitySaveChangesInterceptor);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assemblies = GetConfigAssemblies(typeof(IEntityTypeConfiguration<>));

        Array.ForEach(assemblies, a => modelBuilder.ApplyConfigurationsFromAssembly(a));

        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc/>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    private static Assembly[] GetConfigAssemblies(params Type[] types) =>
       AppDomain.CurrentDomain.GetAssemblies()
           .Where(a => !a.IsDynamic)
           .Where(a => a.GetTypes()
               .Any(t => !t.IsAbstract &&
                         !t.IsInterface &&
                         t.GetInterfaces()
                             .Any(i => i.IsGenericType && types.ToArray().Contains(i.GetGenericTypeDefinition()))))
           .ToArray();
}
