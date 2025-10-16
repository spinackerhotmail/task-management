using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementService.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> logger;
    private readonly ApplicationDbContext context;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context
        )
    {
        this.logger = logger;
        this.context = context;
    }

    /// <summary>
    /// Инициализация
    /// </summary>
    public async Task InitialiseAsync()
    {
        try
        {
            if (context.Database.IsNpgsql())
            {
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        
    }

}
