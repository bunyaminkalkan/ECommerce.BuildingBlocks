using ECommerce.BuildingBlocks.EventBus.Base.Abstractions;
using ECommerce.BuildingBlocks.EventBus.UnitTest.Events.Events;
using MassTransit;
using System.Collections.Concurrent;

namespace ECommerce.BuildingBlocks.EventBus.UnitTest.Events.EventHandlers;

public class OrderCreatedEventHandler : IIntegrationEventHandler<OrderCreatedEvent>
{
    public static readonly ConcurrentBag<Guid> HandledEventIds = new();

    public Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        HandledEventIds.Add(context.Message.Id);
        Console.WriteLine($"Event consumed by OrderCreatedEventHandler: {context.Message.Id}");
        return Task.CompletedTask;
    }
}