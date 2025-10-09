using ECommerce.BuildingBlocks.EventBus.Base.Events;

namespace ECommerce.BuildingBlocks.EventBus.UnitTest.Events.Events;
public class OrderCreatedEvent : IntegrationEvent
{
    public int Id { get; set; }

    public OrderCreatedEvent(int id)
    {
        Id = id;
    }
}
