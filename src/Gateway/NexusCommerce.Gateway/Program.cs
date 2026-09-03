using NexusCommerce.Gateway.PlatformHealth;
using NexusCommerce.Gateway.PlatformHealth.Services;
using NexusCommerce.Gateway.RabbitMq;
using NexusCommerce.Gateway.Jaeger;
using NexusCommerce.Gateway.RabbitMq.Services;
using NexusCommerce.Gateway.Jaeger.Services;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IPlatformHealthService, PlatformHealthService>();
builder.Services.AddScoped<
    IRabbitMqMonitoringService,
    RabbitMqMonitoringService>();

builder.Services.AddScoped<
    IJaegerMonitoringService,
    JaegerMonitoringService>();

builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("Frontend", politica =>
    {
        politica
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(recurso =>
        recurso.AddService(
            serviceName: "NexusCommerce.Gateway",
            serviceVersion: "1.0.0"))
    .WithTracing(trazas =>
    {
        trazas
            .AddAspNetCoreInstrumentation(opciones =>
            {
                opciones.Filter = contexto =>
                    !contexto.Request.Path.StartsWithSegments(
                        "/health");
            })
            .AddHttpClientInstrumentation()
            .AddSource("NexusCommerce.Gateway")
            .AddOtlpExporter(opciones =>
            {
                opciones.Endpoint = new Uri(
                    builder.Configuration[
                        "OpenTelemetry:OtlpEndpoint"]
                    ?? "http://localhost:4317");
            });
    });

var app = builder.Build();

app.UseCors("Frontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => Results.Ok(new
{
    servicio = "NexusCommerce.Gateway",
    estado = "Ejecutándose",
    descripcion = "Punto de entrada de la plataforma NexusCommerce"
}));

app.MapHealthChecks(
    "/health/live",
    new HealthCheckOptions
    {
        Predicate = _ => false
    });

app.MapHealthChecks("/health/ready");

app.MapPlatformHealth();
app.MapRabbitMqMonitoring();
app.MapJaegerMonitoring();
app.MapReverseProxy();

app.Run();
