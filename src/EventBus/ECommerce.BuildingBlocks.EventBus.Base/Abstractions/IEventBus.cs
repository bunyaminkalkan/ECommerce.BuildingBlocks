using ECommerce.BuildingBlocks.EventBus.Base.Events;

namespace ECommerce.BuildingBlocks.EventBus.Base.Abstractions;
public interface IEventBus
{
    void Publish(IntegrationEvent @event);

    void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

    void UnSubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
}
