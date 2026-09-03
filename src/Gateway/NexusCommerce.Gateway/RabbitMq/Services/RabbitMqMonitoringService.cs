using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace NexusCommerce.Gateway.RabbitMq.Services;

public sealed class RabbitMqMonitoringService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
    : IRabbitMqMonitoringService
{
    public async Task<RabbitMqResponse> GetOverviewAsync()
    {
        var managementUrl =
            configuration["RabbitMq:ManagementUrl"]
            ?? "http://rabbitmq:15672";

        var usuario =
            configuration["RabbitMq:Usuario"]
            ?? string.Empty;

        var contrasena =
            configuration["RabbitMq:Contrasena"]
            ?? string.Empty;

        var client = httpClientFactory.CreateClient();

        var credentials =
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    $"{usuario}:{contrasena}"));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Basic",
                credentials);

        using var overviewResponse =
            await client.GetAsync(
                $"{managementUrl}/api/overview");

        overviewResponse.EnsureSuccessStatusCode();

        using var queuesResponse =
            await client.GetAsync(
                $"{managementUrl}/api/queues");

        queuesResponse.EnsureSuccessStatusCode();

        await using var overviewStream =
            await overviewResponse.Content.ReadAsStreamAsync();

        await using var queuesStream =
            await queuesResponse.Content.ReadAsStreamAsync();

        using var overview =
            await JsonDocument.ParseAsync(
                overviewStream);

        using var queues =
            await JsonDocument.ParseAsync(
                queuesStream);

        var root = overview.RootElement;

        var messageStats =
            GetObject(root, "message_stats");

        var objectTotals =
            GetObject(root, "object_totals");

        var queueDetails =
            queues.RootElement
                .EnumerateArray()
                .Select(queue => new RabbitMqQueueResponse
                {
                    Name = GetString(queue, "name"),
                    State = GetString(queue, "state"),
                    Messages = GetInt(queue, "messages"),
                    MessagesReady =
                        GetInt(queue, "messages_ready"),
                    MessagesUnacknowledged =
                        GetInt(
                            queue,
                            "messages_unacknowledged"),
                    Consumers =
                        GetInt(queue, "consumers")
                })
                .OrderBy(queue => queue.Name)
                .ToArray();

        return new RabbitMqResponse
        {
            Status = "Healthy",
            Version =
                GetString(root, "rabbitmq_version"),
            ErlangVersion =
                GetString(root, "erlang_version"),
            ClusterName =
                GetString(root, "cluster_name"),
            Published =
                GetInt(messageStats, "publish"),
            Delivered =
                GetInt(messageStats, "deliver"),
            Acknowledged =
                GetInt(messageStats, "ack"),
            Unroutable =
                GetInt(
                    messageStats,
                    "drop_unroutable"),
            Connections =
                GetInt(
                    objectTotals,
                    "connections"),
            Channels =
                GetInt(
                    objectTotals,
                    "channels"),
            Consumers =
                GetInt(
                    objectTotals,
                    "consumers"),
            Queues =
                GetInt(
                    objectTotals,
                    "queues"),
            CheckedAt = DateTime.UtcNow,
            QueueDetails = queueDetails
        };
    }

    private static JsonElement GetObject(
        JsonElement element,
        string propertyName)
    {
        return element.TryGetProperty(
            propertyName,
            out var property)
            ? property
            : default;
    }

    private static int GetInt(
        JsonElement element,
        string propertyName)
    {
        return element.ValueKind == JsonValueKind.Object &&
               element.TryGetProperty(
                   propertyName,
                   out var property) &&
               property.TryGetInt32(out var value)
            ? value
            : 0;
    }

    private static string GetString(
        JsonElement element,
        string propertyName)
    {
        return element.ValueKind == JsonValueKind.Object &&
               element.TryGetProperty(
                   propertyName,
                   out var property)
            ? property.GetString() ?? string.Empty
            : string.Empty;
    }
}
