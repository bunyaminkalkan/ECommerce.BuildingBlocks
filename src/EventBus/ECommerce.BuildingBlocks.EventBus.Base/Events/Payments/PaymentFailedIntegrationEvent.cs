using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Payments;

public class PaymentFailedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid PaymentId { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerEmail { get; set; }

    public Money Amount { get; set; }
    public string PaymentMethod { get; set; }

    public string FailureReason { get; set; }
    public string ErrorCode { get; set; }
    public DateTime FailedAt { get; set; }
}
