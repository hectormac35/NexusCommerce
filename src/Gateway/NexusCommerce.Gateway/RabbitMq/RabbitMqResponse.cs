namespace NexusCommerce.Gateway.RabbitMq;

public sealed class RabbitMqResponse
{
    public string Status { get; init; } = "Unavailable";
    public string Version { get; init; } = string.Empty;
    public string ErlangVersion { get; init; } = string.Empty;
    public string ClusterName { get; init; } = string.Empty;

    public int Published { get; init; }
    public int Delivered { get; init; }
    public int Acknowledged { get; init; }
    public int Unroutable { get; init; }

    public int Connections { get; init; }
    public int Channels { get; init; }
    public int Consumers { get; init; }
    public int Queues { get; init; }

    public DateTime CheckedAt { get; init; }

    public IReadOnlyCollection<RabbitMqQueueResponse> QueueDetails { get; init; } =
        Array.Empty<RabbitMqQueueResponse>();
}

public sealed class RabbitMqQueueResponse
{
    public string Name { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;

    public int Messages { get; init; }
    public int MessagesReady { get; init; }
    public int MessagesUnacknowledged { get; init; }
    public int Consumers { get; init; }
}
