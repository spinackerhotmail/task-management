using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace TaskManagementService.CommonLib.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"]
            .FirstOrDefault() ?? Guid.NewGuid().ToString();

        context.Response.Headers.Append("X-Correlation-ID", correlationId);

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next(context);
        }
    }
}
