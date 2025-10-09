using ECommerce.BuildingBlocks.EventBus.Base;
using ECommerce.BuildingBlocks.EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace ECommerce.BuildingBlocks.EventBus.RabbitMQ;
public class EventBusRabbitMQ : BaseEventBus
{
    RabbitMQPersistentConnection persistentConnection;
    private readonly IConnectionFactory connectionFactory;
    private IChannel consumerChannel;

    public EventBusRabbitMQ(EventBusConfig config, IServiceProvider serviceProvider) : base(config, serviceProvider)
    {
        if (EventBusConfig.Connection != null)
        {
            if (EventBusConfig.Connection is ConnectionFactory)
                connectionFactory = EventBusConfig.Connection as ConnectionFactory;
            else
            {
                var connJson = JsonConvert.SerializeObject(EventBusConfig.Connection, new JsonSerializerSettings()
                {
                    // Self referencing loop detected for property 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connJson);
            }
        }
        else
            connectionFactory = new ConnectionFactory(); //Create with default values

        persistentConnection = new RabbitMQPersistentConnection(connectionFactory, config.ConnectionRetryCount);

        SubsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }

    public static async Task<EventBusRabbitMQ> CreateAsync(EventBusConfig config, IServiceProvider serviceProvider)
    {
        var instance = new EventBusRabbitMQ(config, serviceProvider);
        instance.consumerChannel = await instance.CreateConsumerChannelAsync();
        return instance;
    }

    private async Task SubsManager_OnEventRemoved(object sender, string eventName)
    {
        eventName = ProcessEventName(eventName);

        if (!persistentConnection.IsConnected)
        {
            await persistentConnection.TryConnectAsync();
        }

        await consumerChannel.QueueUnbindAsync(queue: eventName,
            exchange: EventBusConfig.DefaultTopicName,
            routingKey: eventName);

        if (SubsManager.IsEmpty)
        {
            await consumerChannel.CloseAsync();
        }
    }

    public override async Task PublishAsync(IntegrationEvent @event)
    {
        if (!persistentConnection.IsConnected)
        {
            await persistentConnection.TryConnectAsync();
        }

        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetryAsync(EventBusConfig.ConnectionRetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    // log: Publishing retry attempt due to {ex.Message}
                });

        var eventName = @event.GetType().Name;
        eventName = ProcessEventName(eventName);

        await consumerChannel.ExchangeDeclareAsync(
            exchange: EventBusConfig.DefaultTopicName,
            type: "direct",
            durable: true,
            autoDelete: false,
            arguments: null);

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        await policy.ExecuteAsync(async () =>
        {
            var properties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent, // Persistent delivery mode
                ContentType = "application/json",
                ContentEncoding = "utf-8"
            };

            await consumerChannel.BasicPublishAsync(
                exchange: EventBusConfig.DefaultTopicName,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body);
        });
    }

    public override async Task SubscribeAsync<T, TH>()
    {
        var eventName = typeof(T).Name;
        eventName = ProcessEventName(eventName);

        if (!SubsManager.HasSubscriptionsForEvent(eventName))
        {
            if (!persistentConnection.IsConnected)
            {
                await persistentConnection.TryConnectAsync();
            }

            await consumerChannel.QueueDeclareAsync(queue: GetSubName(eventName), // Ensure queue exists while consuming
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            await consumerChannel.QueueBindAsync(queue: GetSubName(eventName),
                              exchange: EventBusConfig.DefaultTopicName,
                              routingKey: eventName);
        }

        SubsManager.AddSubscription<T, TH>();
        await StartBasicConsumeAsync(eventName);
    }

    public override void UnSubscribe<T, TH>()
    {
        SubsManager.RemoveSubscription<T, TH>();
    }


    private async Task<IChannel> CreateConsumerChannelAsync()
    {
        if (!persistentConnection.IsConnected)
        {
            await persistentConnection.TryConnectAsync();
        }

        var channel = await persistentConnection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: EventBusConfig.DefaultTopicName,
                                type: "direct");

        return channel;
    }

    private async Task StartBasicConsumeAsync(string eventName)
    {
        if (consumerChannel != null)
        {
            var consumer = new AsyncEventingBasicConsumer(consumerChannel);

            consumer.ReceivedAsync += Consumer_ReceivedAsync;

            await consumerChannel.BasicConsumeAsync(
                queue: GetSubName(eventName),
                autoAck: false,
                consumer: consumer);
        }
    }

    private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        eventName = ProcessEventName(eventName);
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            await ProcessEvent(eventName, message);
        }
        catch (Exception ex)
        {
            // logging
        }

        await consumerChannel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
    }
}
