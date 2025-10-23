using ECommerce.BuildingBlocks.EventBus.Base.DTOs;
using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Baskets;

public class BasketCheckedOutIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid CustomerId { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerName { get; set; }

    public List<BasketItemDto> Items { get; set; }

    public Address ShippingAddress { get; set; }
    public Address BillingAddress { get; set; }
}
