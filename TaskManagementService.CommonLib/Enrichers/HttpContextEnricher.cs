using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace TaskManagementService.CommonLib.Enrichers;

/// <summary>
/// Enricher для добавления информации о HTTP контексте в глобальные логи
/// </summary>
public class HttpContextEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        // Добавляем только если это НЕ HTTP request log (избегаем дублирования)
        if (logEvent.MessageTemplate.Text.Contains("HTTP {RequestMethod}")) return;

        // Добавляем контекстную информацию для всех логов
        if (!string.IsNullOrEmpty(httpContext.TraceIdentifier))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "TraceId", httpContext.TraceIdentifier));
        }

        // Добавляем информацию о пользователе (если доступна)
        if (httpContext.User?.Identity?.IsAuthenticated == true)
        {
            var userId = httpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(userId))
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "AuthenticatedUserId", userId));
            }
        }

        // Добавляем IP адрес для логов безопасности
        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString();
        if (!string.IsNullOrEmpty(clientIp))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "ClientIP", clientIp));
        }
    }
}
