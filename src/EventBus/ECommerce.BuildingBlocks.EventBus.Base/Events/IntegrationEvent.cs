namespace ECommerce.BuildingBlocks.EventBus.Base.Events;

public abstract class IntegrationEvent
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
}