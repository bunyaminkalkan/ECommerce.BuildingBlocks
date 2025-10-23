namespace ECommerce.BuildingBlocks.EventBus.Base.DTOs;

public class ReleasedStockItemDto
{
    public Guid ProductId { get; set; }
    public int ReleasedQuantity { get; set; }
}
