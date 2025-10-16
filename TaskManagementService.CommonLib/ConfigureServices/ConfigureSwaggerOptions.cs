using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using TaskManagementService.CommonLib.Services;
using TaskManagementService.CommonLib.Swagger.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManagementService.CommonLib.ConfigureServices
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly INamesProvider systemNames;
        private readonly IApiVersionDescriptionProvider provider;
        private readonly IOptions<ApiVersioningOptions> options;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider, 
            IOptions<ApiVersioningOptions> options,
            INamesProvider systemNames)
        {
            this.provider = provider;
            this.options = options;
            this.systemNames = systemNames;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var apiPath = BuildXmlPath();
            var infraPath = BuildXmlPath("Infrastructure");
            var domainPath = BuildXmlPath("Domain");
            var appPath = BuildXmlPath("Application");

            if (File.Exists(apiPath)) options.IncludeXmlComments(apiPath, includeControllerXmlComments: true);
            if (File.Exists(infraPath)) options.IncludeXmlComments(infraPath);
            if (File.Exists(domainPath)) options.IncludeXmlComments(domainPath);
            if (File.Exists(appPath)) options.IncludeXmlComments(appPath);

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }

            options.SchemaFilter<RequiredPropertiesSchemaFilter>(true);
            options.OperationFilter<CamelCaseParameOperationFilter>();
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = $"{systemNames.ServiceName} API {description.GroupName}",
                Version = description.ApiVersion.ToString()
            };

            if (description.ApiVersion == options.Value.DefaultApiVersion)
            {
                if (!string.IsNullOrWhiteSpace(info.Description))
                {
                    info.Description += " ";
                }

                info.Description += "Default API version.";
            }

            if (description.IsDeprecated)
            {
                if (!string.IsNullOrWhiteSpace(info.Description))
                {
                    info.Description += " ";
                }

                info.Description += "This API version is deprecated.";
            }

            return info;
        }

        private string BuildXmlPath(string? suffix = null)
        {
            var basePath = AppContext.BaseDirectory;
            var fileName = $"{systemNames.CompanyNamespace}.{systemNames.ServiceNamespace}" + (suffix is null ? string.Empty : $".{suffix}") + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }
}
