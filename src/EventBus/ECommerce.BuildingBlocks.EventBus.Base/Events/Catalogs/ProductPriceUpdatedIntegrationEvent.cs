using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.Events.Catalogs;

public class ProductPriceUpdatedIntegrationEvent : IntegrationEvent
{
    public DateTime OccurredOn { get; set; }
    public Guid ProductId { get; set; }
    public Money OldPrice { get; set; }
    public Money NewPrice { get; set; }
    public ProductPriceUpdatedIntegrationEvent(Guid productId, Money oldPrice, Money newPrice)
    {
        ProductId = productId;
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }
}
