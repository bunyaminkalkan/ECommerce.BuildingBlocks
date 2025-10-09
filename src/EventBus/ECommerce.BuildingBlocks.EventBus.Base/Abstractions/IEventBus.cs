using ECommerce.BuildingBlocks.EventBus.Base.Events;

namespace ECommerce.BuildingBlocks.EventBus.Base.Abstractions;
public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event);

    Task SubscribeAsync<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

    void UnSubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
}
