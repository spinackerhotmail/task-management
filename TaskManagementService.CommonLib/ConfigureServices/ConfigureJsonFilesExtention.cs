using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TaskManagementService.CommonLib.ConfigureServices;

public static class ConfigureJsonFilesExtention
{
    public static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder builder, IWebHostEnvironment webHostEnvironment, string fileName = "appsettings")
    {
        builder
            .AddJsonFile(fileName + ".json", optional: false, reloadOnChange: true)
            .AddJsonFile(fileName + ".Local.json", optional: true, reloadOnChange: true)
            .AddJsonFile(fileName + "." + webHostEnvironment.EnvironmentName + ".json", optional: true)
            .AddEnvironmentVariables();

        return builder;
    }
}
