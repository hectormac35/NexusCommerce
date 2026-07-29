using System.Diagnostics;

namespace NexusCommerce.Gateway.PlatformHealth.Services;

public sealed class PlatformHealthService(
    IHttpClientFactory httpClientFactory)
    : IPlatformHealthService
{
    public async Task<PlatformHealthResponse> GetHealthAsync()
    {
        var client = httpClientFactory.CreateClient();

        var services = new List<ServiceHealth>();

        services.Add(await Check(
            client,
            "Gateway",
            "http://localhost:8080/health/ready"));

        services.Add(await Check(
            client,
            "Identity",
            "http://identity-api:8080/health/ready"));

        services.Add(await Check(
            client,
            "Catalog",
            "http://catalog-api:8080/health/ready"));


        services.Add(await Check(
            client,
            "RabbitMQ",
            "http://rabbitmq:15672/"));

        services.Add(await Check(
            client,
            "Jaeger",
            "http://jaeger:16686/"));

        return new PlatformHealthResponse
        {
            Status = services.All(s => s.Status == "Healthy")
                ? "Healthy"
                : "Degraded",

            CheckedAt = DateTime.UtcNow,

            Services = services
        };
    }

    private static async Task<ServiceHealth> Check(
        HttpClient client,
        string name,
        string url)
    {
        var watch = Stopwatch.StartNew();

        try
        {
            var response = await client.GetAsync(url);

            watch.Stop();

            return new ServiceHealth
            {
                Name = name,
                Status = response.IsSuccessStatusCode
                    ? "Healthy"
                    : "Unhealthy",

                ResponseTimeMs = Math.Max(
                    watch.ElapsedMilliseconds,
                    1)
            };
        }
        catch
        {
            watch.Stop();

            return new ServiceHealth
            {
                Name = name,
                Status = "Unavailable",
                ResponseTimeMs = Math.Max(
                    watch.ElapsedMilliseconds,
                    1)
            };
        }
    }
}
