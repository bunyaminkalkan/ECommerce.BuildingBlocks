using ECommerce.BuildingBlocks.EventBus.Base;
using ECommerce.BuildingBlocks.EventBus.Base.Abstractions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.BuildingBlocks.EventBus.RabbitMQ;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds RabbitMQ EventBus with MassTransit to the service collection
    /// </summary>
    public static IServiceCollection AddRabbitMQEventBus(
        this IServiceCollection services,
        EventBusConfig config,
        Action<IBusRegistrationConfigurator>? configureConsumers = null,
        Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>? configureEndpoints = null)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config));

        services.AddSingleton(config);

        services.AddMassTransit(x =>
        {
            // Register consumers automatically if provided
            configureConsumers?.Invoke(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                // Configure RabbitMQ connection
                if (!string.IsNullOrEmpty(config.ConnectionString))
                {
                    cfg.Host(config.ConnectionString);
                }
                else
                {
                    cfg.Host(config.HostName, config.Port, config.VirtualHost, h =>
                    {
                        h.Username(config.UserName);
                        h.Password(config.Password);
                    });
                }

                // Configure message retry
                if (config.EnableRetry)
                {
                    cfg.UseMessageRetry(r => r.Interval(
                        config.RetryCount,
                        TimeSpan.FromSeconds(config.RetryIntervalSeconds)));
                }

                // Configure circuit breaker
                if (config.EnableCircuitBreaker)
                {
                    cfg.UseCircuitBreaker(cb =>
                    {
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });
                }

                // Configure rate limiter
                cfg.UseRateLimit(config.PrefetchCount);

                // Set request timeout
                cfg.UseTimeout(t => t.Timeout = TimeSpan.FromSeconds(config.RequestTimeoutSeconds));

                // Default endpoint configuration (auto-generated queue per consumer)
                cfg.ConfigureEndpoints(context);

                // Allow external endpoint customization (e.g. for tests)
                configureEndpoints?.Invoke(cfg, context);

                // Set prefetch count
                cfg.PrefetchCount = config.PrefetchCount;
            });
        });

        services.AddScoped<IEventBus, EventBusRabbitMQ>();

        return services;
    }

    /// <summary>
    /// Adds RabbitMQ EventBus with automatic consumer discovery from specified assemblies
    /// </summary>
    public static IServiceCollection AddRabbitMQEventBus(
        this IServiceCollection services,
        EventBusConfig config,
        Assembly[] assemblies,
        Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>? configureEndpoints = null)
    {
        return services.AddRabbitMQEventBus(config, x =>
        {
            foreach (var assembly in assemblies)
                x.AddConsumers(assembly);
        }, configureEndpoints);
    }

    /// <summary>
    /// Adds RabbitMQ EventBus with automatic consumer discovery from specified types
    /// </summary>
    public static IServiceCollection AddRabbitMQEventBus(
        this IServiceCollection services,
        EventBusConfig config,
        Type[] consumerTypes,
        Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>? configureEndpoints = null)
    {
        return services.AddRabbitMQEventBus(config, x =>
        {
            foreach (var consumerType in consumerTypes)
                x.AddConsumer(consumerType);
        }, configureEndpoints);
    }
}
