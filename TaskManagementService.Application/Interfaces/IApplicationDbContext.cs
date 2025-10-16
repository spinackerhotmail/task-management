using Microsoft.EntityFrameworkCore;
using TaskManagementService.Domain.Entities;

namespace TaskManagementService.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TaskEntity> TaskEntities { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
