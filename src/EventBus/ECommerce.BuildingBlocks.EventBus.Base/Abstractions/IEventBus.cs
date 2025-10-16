using ECommerce.BuildingBlocks.EventBus.Base.Events;

namespace ECommerce.BuildingBlocks.EventBus.Base.Abstractions;

public interface IEventBus
{
    /// <summary>
    /// Publishes an integration event to the message bus
    /// </summary>
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : IntegrationEvent;

    /// <summary>
    /// Sends a command/message directly to a specific endpoint
    /// </summary>
    Task SendAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : IntegrationEvent;
}