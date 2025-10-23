using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Payments;

public class PaymentSucceededIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid PaymentId { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }

    public Money Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string TransactionId { get; set; }

    public DateTime ProcessedAt { get; set; }
}
