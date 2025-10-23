using ECommerce.BuildingBlocks.EventBus.Base.DTOs;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Inventories;

public class StockReservedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid CorrelationId { get; set; }

    public Guid ReservationId { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }

    public List<ReservedStockItemDto> ReservedItems { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}
