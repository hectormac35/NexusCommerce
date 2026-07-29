namespace NexusCommerce.Gateway.PlatformHealth;

public sealed class PlatformHealthResponse
{
    public required string Status { get; init; }

    public required DateTime CheckedAt { get; init; }

    public required List<ServiceHealth> Services { get; init; }
}

public sealed class ServiceHealth
{
    public required string Name { get; init; }

    public required string Status { get; init; }

    public required long ResponseTimeMs { get; init; }
}
