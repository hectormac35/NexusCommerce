using NexusCommerce.Gateway.Jaeger.Services;

namespace NexusCommerce.Gateway.Jaeger;

public static class JaegerEndpoints
{
    public static void MapJaegerMonitoring(
        this WebApplication app)
    {
        app.MapGet(
            "/api/platform/jaeger",
            async (
                IJaegerMonitoringService service) =>
            {
                try
                {
                    var response =
                        await service.GetOverviewAsync();

                    return Results.Ok(response);
                }
                catch
                {
                    return Results.Problem(
                        title: "Jaeger no disponible",
                        detail:
                            "No se ha podido obtener información del sistema de trazabilidad distribuida.",
                        statusCode:
                            StatusCodes.Status503ServiceUnavailable);
                }
            });
    }
}
