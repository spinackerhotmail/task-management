using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Serilog;
using TaskManagementService.Application;
using TaskManagementService.CommonLib.ConfigureServices;
using TaskManagementService.CommonLib.Extentions;
using TaskManagementService.CommonLib.Middlewares;
using TaskManagementService.Infrastructure;
using TaskManagementService.Spec;

namespace TaskManagementService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
                Log.Information($"Starting {Consts.ServiceNamespace}");

                var builder = WebApplication.CreateBuilder(args);

                builder.Configuration.AddJsonFiles(builder.Environment);
                builder.Host.AddLogger(Consts.ServiceName);

                builder.Services.AddCoreAPIServices(builder.Configuration, names =>
                {
                    names.CompanyNamespace = Consts.CompanyNamespace;
                    names.ServiceNamespace = Consts.ServiceNamespace;
                    names.ServiceName = Consts.ServiceName;
                    names.RoutePrefix = Consts.RoutePrefix;
                });

                builder.Services.AddApplicationServices(builder.Configuration);
                builder.Services.AddInfrastructureServices(builder.Configuration);

                var app = builder.Build();

                app.UseMiddleware<CorrelationIdMiddleware>();

                app.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                app.UseCors();
                app.UseHttpsRedirection();
                app.UseSwaggerService();

                await app.UseDatabase();

                app.UseRouting();

                app.MapControllers();
                app.Run();

            }
            catch (Exception ex)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .CreateLogger();
                Log.Logger.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }
    }
}
