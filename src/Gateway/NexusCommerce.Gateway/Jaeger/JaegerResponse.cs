namespace NexusCommerce.Gateway.Jaeger;

public sealed class JaegerResponse
{
    public string Status { get; init; } = "Unavailable";

    public int ServiceCount { get; init; }
    public int TraceCount { get; init; }
    public int ErrorCount { get; init; }

    public double AverageDurationMs { get; init; }

    public DateTime CheckedAt { get; init; }

    public IReadOnlyCollection<string> Services { get; init; } =
        Array.Empty<string>();

    public IReadOnlyCollection<JaegerTraceResponse> RecentTraces { get; init; } =
        Array.Empty<JaegerTraceResponse>();
}

public sealed class JaegerTraceResponse
{
    public string TraceId { get; init; } = string.Empty;
    public string RootService { get; init; } = string.Empty;
    public string Operation { get; init; } = string.Empty;

    public double DurationMs { get; init; }

    public int SpanCount { get; init; }

    public IReadOnlyCollection<string> Services { get; init; } =
        Array.Empty<string>();

    public bool HasError { get; init; }

    public DateTime StartedAt { get; init; }
}
