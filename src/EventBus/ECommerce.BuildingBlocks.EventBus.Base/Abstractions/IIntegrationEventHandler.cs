using ECommerce.BuildingBlocks.EventBus.Base.Events;
using MassTransit;

namespace ECommerce.BuildingBlocks.EventBus.Base.Abstractions;

/// <summary>
/// Marker interface for integration event handlers
/// This is a wrapper around MassTransit's IConsumer for backward compatibility
/// </summary>
public interface IIntegrationEventHandler<in TIntegrationEvent> : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
}