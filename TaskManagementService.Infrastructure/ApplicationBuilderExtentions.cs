using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementService.Infrastructure.Persistence;

namespace TaskManagementService.Infrastructure
{
    public static class ApplicationBuilderExtentions
    {
        public async static Task<IApplicationBuilder> UseDatabase(this IApplicationBuilder builder)
        {
            if (builder is WebApplication app)
            {
                using var scope = app.Services.CreateScope();

                var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
            }
            return builder;
        }
    }
}
