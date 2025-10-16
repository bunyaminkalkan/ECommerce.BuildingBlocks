using ECommerce.BuildingBlocks.EventBus.Base.Events;

namespace ECommerce.BuildingBlocks.EventBus.UnitTest.Events.Events;

public class OrderCreatedEvent : IntegrationEvent
{
    public int OrderIdValue { get; }

    public OrderCreatedEvent(int orderIdValue)
    {
        OrderIdValue = orderIdValue;
    }
}