using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TaskManagementService.CommonLib.Behaviours;

/// <summary>
/// Pipeline behavior для детального логирования MediatR команд и запросов
/// </summary>
public class DetailedLoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<DetailedLoggingBehaviour<TRequest, TResponse>> logger;

    public DetailedLoggingBehaviour(ILogger<DetailedLoggingBehaviour<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Starting {RequestName}", requestName);

        try
        {
            var response = await next();
            stopwatch.Stop();

            var logLevel = stopwatch.ElapsedMilliseconds > 500
                ? LogLevel.Warning
                : LogLevel.Information;

            logger.Log(logLevel, "Completed {RequestName} in {ElapsedMilliseconds}ms",
                requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex, "Failed {RequestName} after {ElapsedMilliseconds}ms",
                requestName, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
