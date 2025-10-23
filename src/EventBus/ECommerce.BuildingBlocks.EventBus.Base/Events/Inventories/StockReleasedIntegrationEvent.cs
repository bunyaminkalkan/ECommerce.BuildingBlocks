using ECommerce.BuildingBlocks.EventBus.Base.DTOs;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Inventories;

public class StockReleasedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid ReservationId { get; set; }
    public Guid OrderId { get; set; }

    public List<ReleasedStockItemDto> ReleasedItems { get; set; }
    public string ReleaseReason { get; set; }
    public DateTime ReleasedAt { get; set; }
}
