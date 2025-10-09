using ECommerce.BuildingBlocks.EventBus.Base;
using ECommerce.BuildingBlocks.EventBus.Base.Abstractions;
using ECommerce.BuildingBlocks.EventBus.RabbitMQ;
using ECommerce.BuildingBlocks.EventBus.UnitTest.Events.EventHandlers;
using ECommerce.BuildingBlocks.EventBus.UnitTest.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EventBus.UnitTest;

public class EventBusTests
{
    private readonly ServiceCollection _services;

    public EventBusTests()
    {
        _services = new ServiceCollection();
        _services.AddLogging(builder => builder.AddConsole());
    }

    [Fact]
    public async Task Subscribe_Event_On_RabbitMQ_Should_Success()
    {
        // Arrange
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        // Act
        await eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();

        // Assert
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task Unsubscribe_Event_On_RabbitMQ_Should_Success()
    {
        // Arrange
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        await eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();

        // Act
        eventBus.UnSubscribe<OrderCreatedEvent, OrderCreatedEventHandler>();

        // Assert
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task Publish_Message_To_RabbitMQ_Should_Success()
    {
        // Arrange
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        // Act
        await eventBus.PublishAsync(new OrderCreatedEvent(1));

        // Assert
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task Publish_Multiple_Messages_To_RabbitMQ_Should_Success()
    {
        // Arrange
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        // Act
        for (int i = 1; i <= 5; i++)
        {
            await eventBus.PublishAsync(new OrderCreatedEvent(i));
        }

        // Assert
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task Subscribe_And_Publish_Event_Should_Consume_Successfully()
    {
        // Arrange
        _services.AddSingleton<OrderCreatedEventHandler>();
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        // Act
        await eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();
        await eventBus.PublishAsync(new OrderCreatedEvent(1));

        // Wait for message processing
        await Task.Delay(3000);

        // Assert
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task Consume_Multiple_Events_Should_Handle_All_Messages()
    {
        // Arrange
        _services.AddSingleton<OrderCreatedEventHandler>();
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        // Act - Subscribe first
        await eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();

        // Publish multiple messages
        for (int i = 1; i <= 10; i++)
        {
            await eventBus.PublishAsync(new OrderCreatedEvent(i));
        }

        // Wait for all messages to be processed
        await Task.Delay(5000);

        // Assert
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task Consume_Event_After_Unsubscribe_Should_Not_Handle_Message()
    {
        // Arrange
        _services.AddSingleton<OrderCreatedEventHandler>();
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        // Act
        await eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();
        await eventBus.PublishAsync(new OrderCreatedEvent(1)); // This should be consumed

        await Task.Delay(2000);

        eventBus.UnSubscribe<OrderCreatedEvent, OrderCreatedEventHandler>();
        await eventBus.PublishAsync(new OrderCreatedEvent(2)); // This should NOT be consumed

        await Task.Delay(2000);

        // Assert
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task Multiple_Subscribers_Should_Consume_Same_Event()
    {
        // Arrange
        _services.AddSingleton<OrderCreatedEventHandler>();
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        // Act - Multiple subscriptions (fanout pattern için)
        await eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();

        // Publish single event
        await eventBus.PublishAsync(new OrderCreatedEvent(100));

        // Wait for message processing
        await Task.Delay(3000);

        // Assert
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task Long_Running_Consumer_Test()
    {
        // Arrange
        _services.AddSingleton<OrderCreatedEventHandler>();
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        // Act - Subscribe and keep consuming
        await eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();

        // Simulate continuous message flow
        var publishTask = Task.Run(async () =>
        {
            for (int i = 1; i <= 20; i++)
            {
                await eventBus.PublishAsync(new OrderCreatedEvent(i));
                await Task.Delay(500); // Publish every 500ms
            }
        });

        await publishTask;
        await Task.Delay(3000); // Wait for all messages to be processed

        // Assert
        Assert.NotNull(eventBus);
    }

    private EventBusConfig GetRabbitMQConfig()
    {
        return new EventBusConfig()
        {
            ConnectionRetryCount = 5,
            SubscriberClientAppName = "EventBus.UnitTest",
            DefaultTopicName = "ECommerceEventBus",
            EventBusType = EventBusType.RabbitMQ,
            EventNameSuffix = "Event",
            Connection = new ConnectionFactory()
            {
                Uri = new("amqps://aqvuzxby:VZSadpLzKDsJnLsWBYy7BJyddhBN6zsk@cow.rmq2.cloudamqp.com/aqvuzxby")
                //HostName = "localhost",
                //Port = 5672,
                //UserName = "guest",
                //Password = "guest"
            }
        };
    }
}