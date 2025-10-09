using ECommerce.BuildingBlocks.EventBus.Base.Events;

namespace ECommerce.BuildingBlocks.EventBus.Base.Abstractions;
public interface IIntegrationEventHandler<TIntegrationEvent> : IntegrationEventHandler where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}

public interface IntegrationEventHandler
{
}