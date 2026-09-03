using System.Text.Json;

namespace NexusCommerce.Gateway.Jaeger.Services;

public sealed class JaegerMonitoringService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
    : IJaegerMonitoringService
{
    private const int TraceLimit = 20;

    public async Task<JaegerResponse> GetOverviewAsync()
    {
        var baseUrl =
            configuration["Jaeger:QueryUrl"]
            ?? "http://jaeger:16686";

        var client = httpClientFactory.CreateClient();

        using var servicesResponse = await client.GetAsync(
            $"{baseUrl.TrimEnd('/')}/api/services");

        servicesResponse.EnsureSuccessStatusCode();

        await using var servicesStream =
            await servicesResponse.Content.ReadAsStreamAsync();

        using var servicesDocument =
            await JsonDocument.ParseAsync(servicesStream);

        var services = ReadServices(servicesDocument.RootElement);

        var traces = new Dictionary<string, ParsedTrace>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var service in services.Where(
                     service =>
                         !string.Equals(
                             service,
                             "jaeger",
                             StringComparison.OrdinalIgnoreCase)))
        {
            var url =
                $"{baseUrl.TrimEnd('/')}/api/traces" +
                $"?service={Uri.EscapeDataString(service)}" +
                $"&limit={TraceLimit}";

            using var tracesResponse =
                await client.GetAsync(url);

            tracesResponse.EnsureSuccessStatusCode();

            await using var tracesStream =
                await tracesResponse.Content.ReadAsStreamAsync();

            using var tracesDocument =
                await JsonDocument.ParseAsync(tracesStream);

            foreach (var trace in ParseTraces(
                         tracesDocument.RootElement))
            {
                traces.TryAdd(trace.TraceId, trace);
            }
        }

        var recentTraces = traces.Values
            .OrderByDescending(trace => trace.StartedAt)
            .Take(TraceLimit)
            .Select(trace => new JaegerTraceResponse
            {
                TraceId = trace.TraceId,
                RootService = trace.RootService,
                Operation = trace.Operation,
                DurationMs = trace.DurationMs,
                SpanCount = trace.SpanCount,
                Services = trace.Services,
                HasError = trace.HasError,
                StartedAt = trace.StartedAt
            })
            .ToArray();

        return new JaegerResponse
        {
            Status = "Healthy",
            ServiceCount = services.Count,
            TraceCount = recentTraces.Length,
            ErrorCount = recentTraces.Count(
                trace => trace.HasError),
            AverageDurationMs = recentTraces.Length == 0
                ? 0
                : Math.Round(
                    recentTraces.Average(
                        trace => trace.DurationMs),
                    2),
            Services = services,
            RecentTraces = recentTraces,
            CheckedAt = DateTime.UtcNow
        };
    }

    private static IReadOnlyCollection<string> ReadServices(
        JsonElement root)
    {
        if (!root.TryGetProperty(
                "data",
                out var data) ||
            data.ValueKind != JsonValueKind.Array)
        {
            return Array.Empty<string>();
        }

        return data
            .EnumerateArray()
            .Where(item =>
                item.ValueKind == JsonValueKind.String)
            .Select(item =>
                item.GetString() ?? string.Empty)
            .Where(service =>
                !string.IsNullOrWhiteSpace(service))
            .OrderBy(service => service)
            .ToArray();
    }

    private static IEnumerable<ParsedTrace> ParseTraces(
        JsonElement root)
    {
        if (!root.TryGetProperty(
                "data",
                out var data) ||
            data.ValueKind != JsonValueKind.Array)
        {
            yield break;
        }

        foreach (var trace in data.EnumerateArray())
        {
            if (!trace.TryGetProperty(
                    "traceID",
                    out var traceIdElement))
            {
                continue;
            }

            var traceId =
                traceIdElement.GetString()
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(traceId))
            {
                continue;
            }

            var processes =
                ReadProcesses(trace);

            var spans =
                trace.TryGetProperty(
                    "spans",
                    out var spansElement) &&
                spansElement.ValueKind ==
                JsonValueKind.Array
                    ? spansElement
                        .EnumerateArray()
                        .ToArray()
                    : Array.Empty<JsonElement>();

            if (spans.Length == 0)
            {
                continue;
            }

            var rootSpan =
                spans.FirstOrDefault(
                    span =>
                        !span.TryGetProperty(
                            "references",
                            out var references) ||
                        references.ValueKind !=
                            JsonValueKind.Array ||
                        references.GetArrayLength() == 0);

            if (rootSpan.ValueKind ==
                JsonValueKind.Undefined)
            {
                rootSpan = spans[0];
            }

            var startedAtMicroseconds =
                spans.Min(ReadStartTime);

            var finishedAtMicroseconds =
                spans.Max(
                    span =>
                        ReadStartTime(span) +
                        ReadDuration(span));

            var durationMicroseconds =
                Math.Max(
                    0,
                    finishedAtMicroseconds -
                    startedAtMicroseconds);

            var services = spans
                .Select(
                    span =>
                        ResolveService(
                            span,
                            processes))
                .Where(service =>
                    !string.IsNullOrWhiteSpace(service))
                .Distinct(
                    StringComparer.OrdinalIgnoreCase)
                .OrderBy(service => service)
                .ToArray();

            yield return new ParsedTrace
            {
                TraceId = traceId,
                RootService =
                    ResolveService(
                        rootSpan,
                        processes),
                Operation =
                    ReadString(
                        rootSpan,
                        "operationName"),
                DurationMs =
                    Math.Round(
                        durationMicroseconds / 1000d,
                        2),
                SpanCount = spans.Length,
                Services = services,
                HasError = spans.Any(HasError),
                StartedAt =
                    DateTime.UnixEpoch.AddTicks(
                        startedAtMicroseconds * 10)
            };
        }
    }

    private static Dictionary<string, string> ReadProcesses(
        JsonElement trace)
    {
        var result =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

        if (!trace.TryGetProperty(
                "processes",
                out var processes) ||
            processes.ValueKind !=
            JsonValueKind.Object)
        {
            return result;
        }

        foreach (var process in
                 processes.EnumerateObject())
        {
            var serviceName =
                ReadString(
                    process.Value,
                    "serviceName");

            result[process.Name] =
                serviceName;
        }

        return result;
    }

    private static string ResolveService(
        JsonElement span,
        IReadOnlyDictionary<string, string> processes)
    {
        var processId =
            ReadString(
                span,
                "processID");

        return processes.TryGetValue(
            processId,
            out var serviceName)
            ? serviceName
            : string.Empty;
    }

    private static bool HasError(
        JsonElement span)
    {
        if (!span.TryGetProperty(
                "tags",
                out var tags) ||
            tags.ValueKind !=
            JsonValueKind.Array)
        {
            return false;
        }

        foreach (var tag in tags.EnumerateArray())
        {
            var key =
                ReadString(
                    tag,
                    "key");

            if (string.Equals(
                    key,
                    "error",
                    StringComparison.OrdinalIgnoreCase) &&
                tag.TryGetProperty(
                    "value",
                    out var value))
            {
                if (value.ValueKind ==
                        JsonValueKind.True)
                {
                    return true;
                }

                if (value.ValueKind ==
                        JsonValueKind.String &&
                    bool.TryParse(
                        value.GetString(),
                        out var error) &&
                    error)
                {
                    return true;
                }
            }

            if (string.Equals(
                    key,
                    "otel.status_code",
                    StringComparison.OrdinalIgnoreCase) &&
                string.Equals(
                    ReadString(tag, "value"),
                    "ERROR",
                    StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static long ReadStartTime(
        JsonElement span)
    {
        return ReadLong(
            span,
            "startTime");
    }

    private static long ReadDuration(
        JsonElement span)
    {
        return ReadLong(
            span,
            "duration");
    }

    private static long ReadLong(
        JsonElement element,
        string property)
    {
        return element.TryGetProperty(
                   property,
                   out var value) &&
               value.TryGetInt64(
                   out var result)
            ? result
            : 0;
    }

    private static string ReadString(
        JsonElement element,
        string property)
    {
        if (!element.TryGetProperty(
                property,
                out var value))
        {
            return string.Empty;
        }

        return value.ValueKind ==
            JsonValueKind.String
            ? value.GetString()
              ?? string.Empty
            : value.ToString();
    }

    private sealed class ParsedTrace
    {
        public string TraceId { get; init; } =
            string.Empty;

        public string RootService { get; init; } =
            string.Empty;

        public string Operation { get; init; } =
            string.Empty;

        public double DurationMs { get; init; }

        public int SpanCount { get; init; }

        public IReadOnlyCollection<string> Services {
            get;
            init;
        } = Array.Empty<string>();

        public bool HasError { get; init; }

        public DateTime StartedAt { get; init; }
    }
}
