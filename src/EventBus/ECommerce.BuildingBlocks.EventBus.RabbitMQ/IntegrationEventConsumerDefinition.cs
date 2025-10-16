using MassTransit;

namespace ECommerce.BuildingBlocks.EventBus.RabbitMQ;

/// <summary>
/// Base consumer definition for integration events
/// Provides common configuration for all event consumers
/// </summary>
public class IntegrationEventConsumerDefinition<TConsumer> : ConsumerDefinition<TConsumer>
    where TConsumer : class, IConsumer
{
    public IntegrationEventConsumerDefinition()
    {
        // Set concurrent message limit per consumer instance
        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // Configure retry policy for this specific consumer
        endpointConfigurator.UseMessageRetry(r => r.Intervals(
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(30)));

        // Configure error handling
        endpointConfigurator.UseInMemoryOutbox(context);

        // Configure circuit breaker
        endpointConfigurator.UseCircuitBreaker(cb =>
        {
            cb.TripThreshold = 15;
            cb.ActiveThreshold = 10;
            cb.ResetInterval = TimeSpan.FromMinutes(5);
        });
    }
}