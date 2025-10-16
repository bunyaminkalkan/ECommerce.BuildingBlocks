namespace ECommerce.BuildingBlocks.EventBus.Base;

public class EventBusConfig
{
    /// <summary>
    /// Connection string for the message broker (RabbitMQ, etc.)
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Host name for RabbitMQ
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// Port for RabbitMQ
    /// </summary>
    public ushort Port { get; set; } = 5672;

    /// <summary>
    /// Username for authentication
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Password for authentication
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Virtual host for RabbitMQ
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Queue name prefix for this service
    /// </summary>
    public string QueueName { get; set; } = string.Empty;

    /// <summary>
    /// Number of concurrent message processing
    /// </summary>
    public int PrefetchCount { get; set; } = 16;

    /// <summary>
    /// Number of retry attempts for connection failures
    /// </summary>
    public int ConnectionRetryCount { get; set; } = 5;

    /// <summary>
    /// Enable retry policy for failed messages
    /// </summary>
    public bool EnableRetry { get; set; } = true;

    /// <summary>
    /// Number of retry attempts for failed message processing
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Initial retry interval in seconds
    /// </summary>
    public int RetryIntervalSeconds { get; set; } = 5;

    /// <summary>
    /// Enable circuit breaker
    /// </summary>
    public bool EnableCircuitBreaker { get; set; } = true;

    /// <summary>
    /// Request timeout in seconds
    /// </summary>
    public int RequestTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Enable message outbox pattern
    /// </summary>
    public bool EnableOutbox { get; set; } = false;

    /// <summary>
    /// Type of event bus to use
    /// </summary>
    public EventBusType EventBusType { get; set; } = EventBusType.RabbitMQ;
}

public enum EventBusType
{
    RabbitMQ = 0
}