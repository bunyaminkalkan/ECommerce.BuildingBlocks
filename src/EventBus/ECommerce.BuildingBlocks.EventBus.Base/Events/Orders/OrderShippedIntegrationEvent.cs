using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Orders;

public class OrderShippedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerEmail { get; set; }

    public string TrackingNumber { get; set; }
    public string CarrierName { get; set; }
    public DateTime ShippedAt { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }

    public Address ShippingAddress { get; set; }
}
