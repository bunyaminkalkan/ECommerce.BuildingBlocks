namespace ECommerce.BuildingBlocks.EventBus.Base.DTOs;

public class FailedStockItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int RequestedQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public string Reason { get; set; }
}
