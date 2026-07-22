using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Notification.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AgregarInfraestructuraNotificaciones(
    builder.Configuration);

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(recurso =>
        recurso.AddService(
            serviceName: "NexusCommerce.Notification",
            serviceVersion: "1.0.0"))
    .WithTracing(trazas =>
    {
        trazas
            .AddSource("NexusCommerce.Notification")
            .AddOtlpExporter(opciones =>
            {
                opciones.Endpoint = new Uri(
                    builder.Configuration[
                        "OpenTelemetry:OtlpEndpoint"]
                    ?? "http://localhost:4317");
            });
    });

var host = builder.Build();

host.Run();
