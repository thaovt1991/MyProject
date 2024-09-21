using System.Threading.Tasks;
using HomemadeCakes.Common.EventBus.Events;

namespace HomemadeCakes.Common.EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
