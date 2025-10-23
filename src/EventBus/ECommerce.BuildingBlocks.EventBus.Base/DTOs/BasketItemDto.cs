using ECommerce.BuildingBlocks.Shared.Kernel.ValueObjects;

namespace ECommerce.BuildingBlocks.EventBus.Base.DTOs;

public class BasketItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public Money UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
}
