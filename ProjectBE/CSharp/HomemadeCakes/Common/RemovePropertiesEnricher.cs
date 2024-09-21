using Serilog.Core;
using Serilog.Events;

namespace HomemadeCakes.Common
{
    internal class RemovePropertiesEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent le, ILogEventPropertyFactory lepf)
        {
            le.RemovePropertyIfPresent("ActionId");
            le.RemovePropertyIfPresent("ActionName");
        }
    }
}
