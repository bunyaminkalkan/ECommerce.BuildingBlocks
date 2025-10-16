namespace ECommerce.BuildingBlocks.EventBus.Base.Events;

public abstract class IntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
}