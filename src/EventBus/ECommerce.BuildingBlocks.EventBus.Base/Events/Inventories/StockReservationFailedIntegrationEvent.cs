using ECommerce.BuildingBlocks.EventBus.Base.DTOs;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Inventories;

public class StockReservationFailedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerEmail { get; set; }

    public List<FailedStockItemDto> FailedItems { get; set; }
    public string FailureReason { get; set; }
}
