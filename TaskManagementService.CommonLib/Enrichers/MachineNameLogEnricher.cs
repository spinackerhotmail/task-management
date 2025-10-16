using Serilog.Core;
using Serilog.Events;

namespace TaskManagementService.CommonLib.Enrichers
{
    public class MachineNameLogEnricher : ILogEventEnricher
    {
        public const string MachineNamePropertyName = "MachineName";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var machineName = Environment.MachineName;

            var enrichProperty = propertyFactory.CreateProperty(MachineNamePropertyName, machineName);

            logEvent.AddOrUpdateProperty(enrichProperty);
        }
    }
}
