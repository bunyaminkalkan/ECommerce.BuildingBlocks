using ECommerce.BuildingBlocks.EventBus.Base.DTOs;
using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Orders;

public class OrderCancelledIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerEmail { get; set; }

    public string CancellationReason { get; set; }
    public string CancelledBy { get; set; } // Customer, Admin, System
    public DateTime CancelledAt { get; set; }

    public List<OrderItemDto> OrderItems { get; set; }
    public Money RefundAmount { get; set; }
    public Guid? PaymentId { get; set; }
}
