using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Payments;

public class PaymentRefundedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid PaymentId { get; set; }
    public Guid RefundId { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerEmail { get; set; }

    public Money RefundAmount { get; set; }
    public string RefundReason { get; set; }
    public string RefundMethod { get; set; }

    public DateTime RefundedAt { get; set; }
}
