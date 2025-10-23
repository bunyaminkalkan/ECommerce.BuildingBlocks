using ECommerce.BuildingBlocks.EventBus.Base.DTOs;
using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Orders;

public class OrderCreatedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerName { get; set; }

    public List<OrderItemDto> OrderItems { get; set; }
    public Money TotalAmount { get; set; }

    public Address ShippingAddress { get; set; }
    public Address BillingAddress { get; set; }

    public string PaymentToken { get; set; }
    public string PaymentMethod { get; set; }
}
