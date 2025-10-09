using ECommerce.BuildingBlocks.EventBus.Base.Abstractions;
using ECommerce.BuildingBlocks.EventBus.UnitTest.Events.Events;

namespace ECommerce.BuildingBlocks.EventBus.UnitTest.Events.EventHandlers;
public class OrderCreatedEventHandler : IIntegrationEventHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent @event)
    {
        return Task.CompletedTask;
    }
}

