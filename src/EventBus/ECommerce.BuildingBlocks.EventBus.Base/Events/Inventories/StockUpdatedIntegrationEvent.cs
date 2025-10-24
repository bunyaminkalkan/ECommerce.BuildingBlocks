namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Inventories;

public class StockUpdatedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid ProductId { get; set; }
    public int QuantityChange { get; set; }
    public StockUpdatedIntegrationEvent(Guid productId, int quantityChange)
    {
        ProductId = productId;
        QuantityChange = quantityChange;
    }
}
