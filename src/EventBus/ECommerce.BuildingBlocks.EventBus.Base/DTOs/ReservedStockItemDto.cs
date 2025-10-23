namespace ECommerce.BuildingBlocks.EventBus.Base.DTOs;

public class ReservedStockItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int ReservedQuantity { get; set; }
}
