using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace ECommerce.BuildingBlocks.EventBus.RabbitMQ;
public class RabbitMQPersistentConnection : IDisposable
{
    private readonly IConnectionFactory connectionFactory;
    private readonly int retryCount;
    private IConnection connection;
    private object lock_object = new object();
    private bool _disposed;
    public bool IsConnected => connection != null && connection.IsOpen;


    public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount = 5)
    {
        this.connectionFactory = connectionFactory;
        this.retryCount = retryCount;
    }


    public async Task<IChannel> CreateChannelAsync()
    {
        return await connection.CreateChannelAsync();
    }

    public void Dispose()
    {
        _disposed = true;
        connection.Dispose();
    }


    public async Task<bool> TryConnectAsync()
    {
        lock (lock_object)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                }
            );

            policy.Execute(async () =>
            {
                connection = await connectionFactory.CreateConnectionAsync();
            });

            if (IsConnected)
            {
                connection.ConnectionShutdownAsync += Connection_ConnectionShutdownAsync;
                connection.CallbackExceptionAsync += Connection_CallbackExceptionAsync;
                connection.ConnectionBlockedAsync += Connection_ConnectionBlockedAsync;
                // log

                return true;
            }

            return false;
        }
    }

    private async Task Connection_ConnectionBlockedAsync(object sender, ConnectionBlockedEventArgs @event)
    {
        if (_disposed) return;
        await TryConnectAsync();
    }

    private async Task Connection_CallbackExceptionAsync(object sender, CallbackExceptionEventArgs @event)
    {
        if (_disposed) return;
        await TryConnectAsync();
    }

    private async Task Connection_ConnectionShutdownAsync(object sender, ShutdownEventArgs @event)
    {
        if (_disposed) return;
        await TryConnectAsync();
    }
}
