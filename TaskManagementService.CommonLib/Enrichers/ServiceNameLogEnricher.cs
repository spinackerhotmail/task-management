using Serilog.Core;
using Serilog.Events;

namespace TaskManagementService.CommonLib.Enrichers
{
    public class ServiceNameLogEnricher : ILogEventEnricher
    {
        private readonly string serviceName;

        public ServiceNameLogEnricher(string serviceName)
        {
            this.serviceName = serviceName;
        }

        public const string ServiceNamePropertyName = "ServiceName";
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var enrichProperty = propertyFactory.CreateProperty(ServiceNamePropertyName, this.serviceName);

            logEvent.AddOrUpdateProperty(enrichProperty);
        }
    }
}
