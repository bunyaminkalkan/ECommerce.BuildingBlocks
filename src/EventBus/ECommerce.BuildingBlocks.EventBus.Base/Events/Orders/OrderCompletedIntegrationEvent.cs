namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Orders;

public class OrderCompletedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime CompletedAt { get; set; }
}
