using Serilog.Core;
using Serilog.Events;

namespace TaskManagementService.CommonLib.Enrichers;

/// <summary>
/// Enricher для добавления информации о производительности
/// </summary>
public class PerformanceEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // Добавляем метрики производительности для warning/error логов
        if (logEvent.Level >= LogEventLevel.Warning)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "Timestamp", DateTimeOffset.UtcNow));

            // Добавляем информацию о памяти для критических логов
            if (logEvent.Level >= LogEventLevel.Error)
            {
                var workingSet = Environment.WorkingSet;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "MemoryUsageMB", workingSet / 1024 / 1024));
            }
        }
    }
}
