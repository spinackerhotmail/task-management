using Microsoft.Extensions.DependencyInjection;
using TaskManagementService.CommonLib.Services;

namespace TaskManagementService.CommonLib.ConfigureServices;

public static class ConfigureNamesProviderExtention
{
    /// <summary>
    /// Пример использования: 
    /// services.AddSystemNames(names => { names.CompanyNamespace = "Shop24"; names.ServiceNamespace = "Orders"; names.ServiceName = "Orders"; names.RoutePrefix = "orders"; });
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureNames"></param>
    /// <returns></returns>
    public static IServiceCollection AddSystemNames(this IServiceCollection services, Action<NamesProvider> configureNames)
    {
        var names = new NamesProvider();
        configureNames(names);
        services.AddSingleton<INamesProvider>(names);

        return services;
    }
}
