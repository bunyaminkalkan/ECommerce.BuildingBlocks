using ECommerce.BuildingBlocks.EventBus.Base.Abstractions;
using ECommerce.BuildingBlocks.EventBus.Base.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ECommerce.BuildingBlocks.EventBus.RabbitMQ;

public class EventBusRabbitMQ : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly ILogger<EventBusRabbitMQ> _logger;

    public EventBusRabbitMQ(
        IPublishEndpoint publishEndpoint,
        ISendEndpointProvider sendEndpointProvider,
        ILogger<EventBusRabbitMQ> logger)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IntegrationEvent
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event));

        try
        {
            _logger.LogInformation(
                "Publishing event {EventType} with Id: {EventId}",
                typeof(T).Name,
                @event.GetType().GetProperty("Id")?.GetValue(@event));

            await _publishEndpoint.Publish(@event, cancellationToken);

            _logger.LogInformation(
                "Successfully published event {EventType}",
                typeof(T).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error publishing event {EventType}",
                typeof(T).Name);
            throw;
        }
    }

    public async Task SendAsync<T>(T message, CancellationToken cancellationToken = default) where T : IntegrationEvent
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        try
        {
            _logger.LogInformation(
                "Sending message {MessageType}",
                typeof(T).Name);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{typeof(T).Name}"));
            await endpoint.Send(message, cancellationToken);

            _logger.LogInformation(
                "Successfully sent message {MessageType}",
                typeof(T).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error sending message {MessageType}",
                typeof(T).Name);
            throw;
        }
    }
}