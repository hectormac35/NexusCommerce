using NexusCommerce.Gateway.RabbitMq.Services;

namespace NexusCommerce.Gateway.RabbitMq;

public static class RabbitMqEndpoints
{
    public static void MapRabbitMqMonitoring(
        this WebApplication app)
    {
        app.MapGet(
            "/api/platform/rabbitmq",
            async (
                IRabbitMqMonitoringService service) =>
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
                        title: "RabbitMQ no disponible",
                        detail:
                            "No se ha podido obtener información del broker RabbitMQ.",
                        statusCode:
                            StatusCodes.Status503ServiceUnavailable);
                }
            });
    }
}
