using Microsoft.Extensions.DependencyInjection;

namespace TaskManagementService.CommonLib.ConfigureServices;

public static class ConfigureSwaggerExtention
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        return services;
    }
}
