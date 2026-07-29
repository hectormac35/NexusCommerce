using NexusCommerce.Gateway.PlatformHealth.Services;

namespace NexusCommerce.Gateway.PlatformHealth;

public static class PlatformHealthEndpoints
{
    public static void MapPlatformHealth(this WebApplication app)
    {
        app.MapGet(
            "/api/platform/health",
            async (
                IPlatformHealthService platformHealthService) =>
            {
                var response =
                    await platformHealthService
                        .GetHealthAsync();

                return Results.Ok(response);
            });
    }
}
