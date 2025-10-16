using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementService.CommonLib.Services;

namespace TaskManagementService.CommonLib.Extentions;

public static class ApplicationBuilderExtentions
{
    public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder builder)
    {
        var serviceProvider = builder.ApplicationServices;
        var nameProvider = serviceProvider.GetRequiredService<INamesProvider>();

        builder.UseSwagger(options => options.RouteTemplate = $"/{nameProvider.RoutePrefix}" + "/swagger/{documentName}/swagger.json");
        builder.UseSwaggerUI(options =>
        {
            var serviceProvider = builder.ApplicationServices;
            var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
            var nameProvider = serviceProvider.GetRequiredService<INamesProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.RoutePrefix = $"{nameProvider.RoutePrefix}/swagger";
                options.SwaggerEndpoint($"/{options.RoutePrefix}/{description.GroupName}/swagger.json", $"{nameProvider.ServiceName} {description.GroupName.ToUpperInvariant()}");
            }
        });

        return builder;
    }

}
