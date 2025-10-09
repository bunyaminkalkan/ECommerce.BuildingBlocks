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
        // Subscription should complete without throwing exception
        Assert.NotNull(eventBus);
    }

    [Fact]
    public void Unsubscribe_Event_On_RabbitMQ_Should_Success()
    {
        // Arrange
        _services.AddSingleton<IEventBus>(sp =>
        {
            return EventBusRabbitMQ.CreateAsync(GetRabbitMQConfig(), sp)
                                   .GetAwaiter().GetResult();
        });

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();

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
        // Publish should complete without throwing exception
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
    public async Task Subscribe_And_Publish_Event_Test()
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
        await eventBus.PublishAsync(new OrderCreatedEvent(1));

        // Wait for message processing
        Thread.Sleep(2000);

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
                //Uri = new("")
                //HostName = "localhost",
                //Port = 5672,
                //UserName = "guest",
                //Password = "guest"
            }
        };
    }
}