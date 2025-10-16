using Serilog.Core;
using Serilog.Events;

namespace TaskManagementService.CommonLib.Enrichers;

/// <summary>
/// Enricher для добавления категории логов
/// </summary>
public class LogCategoryEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // Определяем категорию лога на основе источника
        var sourceContext = logEvent.Properties.GetValueOrDefault("SourceContext")?.ToString();
        if (string.IsNullOrEmpty(sourceContext)) return;

        var category = DetermineLogCategory(sourceContext);
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "LogCategory", category));
    }

    private static string DetermineLogCategory(string sourceContext)
    {
        return sourceContext.ToLowerInvariant() switch
        {
            var s when s.Contains("controller") => "API",
            var s when s.Contains("tokenaudit") => "Security",
            var s when s.Contains("auth") => "Authentication",
            var s when s.Contains("mediator") => "BusinessLogic",
            var s when s.Contains("entityframework") => "Database",
            var s when s.Contains("httpclient") => "ExternalAPI",
            var s when s.Contains("performance") => "Performance",
            _ => "General"
        };
    }
}
