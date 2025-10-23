using ECommerce.BuildingBlocks.EventBus.Base.DTOs;
using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Orders;

public class OrderPaidIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid PaymentId { get; set; }
    public Money PaidAmount { get; set; }
    public DateTime PaidAt { get; set; }

    public List<OrderItemDto> OrderItems { get; set; }
}
