using Asp.Versioning;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManagementService.CommonLib.Behaviours;
using TaskManagementService.CommonLib.Filters;
using TaskManagementService.CommonLib.Interceprots;
using TaskManagementService.CommonLib.Services;

namespace TaskManagementService.CommonLib.ConfigureServices;

public static class ConfigureCoreServicesExtention
{
    public static IServiceCollection AddCoreAPIServices(this IServiceCollection services, IConfiguration configuration, Action<NamesProvider> configureNames)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(configureNames);

        services.AddSystemNames(configureNames);

        // Собираем все загруженные в домен приложения сборки (для AutoMapper и MediatR)
        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.FullName))
            .ToArray();

        services.AddAutoMapper(assemblies);

        services.AddMediatR(cfg =>
        {
            cfg.TypeEvaluator = x => !x.ContainsGenericParameters;
            cfg.RegisterServicesFromAssemblies(assemblies);
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DetailedLoggingBehaviour<,>));

        services.AddHttpContextAccessor();
        services.AddHealthChecks();
        services.AddCors(options => options
                    .AddDefaultPolicy(policyBuilder => policyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()));

        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilterAttribute>();
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
             .AddMvc(options => { })
             .AddApiExplorer(options =>
             {
                 options.GroupNameFormat = "'v'VVV";
                 options.SubstituteApiVersionInUrl = true;
             });

        services.AddSwaggerServices();

        services.AddTransient<IDateTime, DateTimeService>();

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        return services;
    }
}
