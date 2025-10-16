using Serilog.Core;
using Serilog.Events;

namespace TaskManagementService.CommonLib.Enrichers
{
    public class LevelLogEnricher : ILogEventEnricher
    {
        public const string LevelPropertyName = "Level";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var enrichProperty = propertyFactory.CreateProperty(LevelPropertyName, logEvent.Level);

            logEvent.AddOrUpdateProperty(enrichProperty);
        }
    }
}
