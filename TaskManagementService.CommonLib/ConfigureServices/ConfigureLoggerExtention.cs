using Serilog;
using Serilog.Formatting.Compact;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TaskManagementService.CommonLib.Enrichers;



namespace TaskManagementService.CommonLib.ConfigureServices;

public static class ConfigureLoggerExtention
{
    public static IHostBuilder AddLogger(this IHostBuilder builder, string serviceName)
    {
        builder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.With(new MachineNameLogEnricher())
                .Enrich.With(new LevelLogEnricher())
                .Enrich.With(new ServiceNameLogEnricher(serviceName))
                .Enrich.With(new HttpContextEnricher((IHttpContextAccessor)services.GetService(typeof(IHttpContextAccessor))!))
                .Enrich.With(new PerformanceEnricher())
                .Enrich.With(new LogCategoryEnricher())
                .WriteTo.Console(new RenderedCompactJsonFormatter());

            if (context.Configuration.GetValue<bool>("Logging:Seq:Enabled"))
            {
                var seqUrl = context.Configuration["Logging:Seq:Url"] ?? "http://localhost:5341";
                configuration.WriteTo.Seq(seqUrl);
            }
        });

        return builder;
    }
}
