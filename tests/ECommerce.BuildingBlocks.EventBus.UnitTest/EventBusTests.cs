using ECommerce.BuildingBlocks.EventBus.Base;
using ECommerce.BuildingBlocks.EventBus.Base.Abstractions;
using ECommerce.BuildingBlocks.EventBus.RabbitMQ;
using ECommerce.BuildingBlocks.EventBus.UnitTest.Events.EventHandlers;
using ECommerce.BuildingBlocks.EventBus.UnitTest.Events.Events;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventBus.UnitTest;

public class EventBusTests : IAsyncLifetime
{
    private ServiceProvider _serviceProvider;
    private IBusControl _busControl;
    private IEventBus _eventBus;

    public async Task InitializeAsync()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());

        services.AddRabbitMQEventBus(
            GetRabbitMQConfig(),
            new[] { typeof(OrderCreatedEventHandler) },
            (cfg, context) =>
            {
                // 👇 SendAsync "queue:OrderCreatedEvent" adresine gönderiyor,
                cfg.ReceiveEndpoint("OrderCreatedEvent", e =>
                {
                    e.ConfigureConsumer<OrderCreatedEventHandler>(context);
                });
            });

        _serviceProvider = services.BuildServiceProvider();

        _busControl = _serviceProvider.GetRequiredService<IBusControl>();
        _eventBus = _serviceProvider.GetRequiredService<IEventBus>();

        OrderCreatedEventHandler.HandledEventIds.Clear();

        await _busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
    }

    public async Task DisposeAsync()
    {
        await _busControl.StopAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
        await _serviceProvider.DisposeAsync();
    }

    private EventBusConfig GetRabbitMQConfig()
    {
        return new EventBusConfig
        {
            ConnectionString = "amqps://aqvuzxby:VZSadpLzKDsJnLsWBYy7BJyddhBN6zsk@cow.rmq2.cloudamqp.com/aqvuzxby",
            ConnectionRetryCount = 5,
            EnableRetry = true,
            RetryCount = 3,
            RetryIntervalSeconds = 10,
            QueueName = $"ECommerce.UnitTestQueue.{Guid.NewGuid()}"
        };
    }

    [Fact]
    public async Task Publish_Single_Event_Should_Be_Consumed_Successfully()
    {
        // Arrange
        var @event = new OrderCreatedEvent(1);

        // Act
        await _eventBus.PublishAsync(@event);

        // Assert
        await Task.Delay(1000);
        Assert.Contains(@event.Id, OrderCreatedEventHandler.HandledEventIds);
    }

    [Fact]
    public async Task Publish_Multiple_Events_Should_All_Be_Consumed()
    {
        // Arrange
        var events = new List<OrderCreatedEvent>
        {
            new (101),
            new (102),
            new (103)
        };
        var eventIds = events.Select(e => e.Id).ToList();

        // Act
        await Task.WhenAll(events.Select(e => _eventBus.PublishAsync(e)));

        // Assert
        await Task.Delay(1000);
        Assert.All(eventIds, id => Assert.Contains(id, OrderCreatedEventHandler.HandledEventIds));
    }

    [Fact]
    public async Task Send_Command_Should_Be_Received_By_Queue()
    {
        // Arrange
        var command = new OrderCreatedEvent(201);

        // Act
        await _eventBus.SendAsync(command);

        // Assert
        await Task.Delay(1000);
        Assert.Contains(command.Id, OrderCreatedEventHandler.HandledEventIds);
    }
}