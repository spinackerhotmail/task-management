using Microsoft.Extensions.DependencyInjection;
using TaskManagementService.CommonLib.Services;

namespace TaskManagementService.CommonLib.ConfigureServices;

public static class ConfigureNamesProviderExtention
{
    public static IServiceCollection AddSystemNames(this IServiceCollection services, Action<NamesProvider> configureNames)
    {
        var names = new NamesProvider();
        configureNames(names);
        services.AddSingleton<INamesProvider>(names);

        return services;
    }
}
