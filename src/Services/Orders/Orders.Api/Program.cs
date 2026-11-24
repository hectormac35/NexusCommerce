using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Orders.Application;
using Orders.Infrastructure;
using Orders.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddOrdersApplication();

builder.Services.AddOrdersInfrastructure(
    builder.Configuration);

var cadenaConexion = builder.Configuration
    .GetConnectionString("OrdersDatabase")
    ?? throw new InvalidOperationException(
        "No se ha configurado la conexión 'OrdersDatabase'.");

builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        cadenaConexion,
        name: "postgres-orders",
        tags: ["ready"]);

// Mismo esquema JWT que Catalog e Identity: el token lo emite Identity.Api.
var jwtClave = builder.Configuration["Jwt:Clave"]
    ?? throw new InvalidOperationException(
        "No se ha configurado 'Jwt:Clave'.");

var jwtEmisor = builder.Configuration["Jwt:Emisor"]
    ?? throw new InvalidOperationException(
        "No se ha configurado 'Jwt:Emisor'.");

var jwtAudiencia = builder.Configuration["Jwt:Audiencia"]
    ?? throw new InvalidOperationException(
        "No se ha configurado 'Jwt:Audiencia'.");

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtEmisor,

                ValidateAudience = true,
                ValidAudience = jwtAudiencia,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtClave)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(recurso =>
        recurso.AddService(
            serviceName: "NexusCommerce.Orders",
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
            .AddEntityFrameworkCoreInstrumentation()
            .AddSource("NexusCommerce.Orders")
            .AddOtlpExporter(opciones =>
            {
                opciones.Endpoint = new Uri(
                    builder.Configuration[
                        "OpenTelemetry:OtlpEndpoint"]
                    ?? "http://localhost:4317");
            });
    });

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (IServiceScope scope = app.Services.CreateScope())
{
    OrdersDbContext dbContext = scope.ServiceProvider
        .GetRequiredService<OrdersDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.MapGet(
    "/",
    () => Results.Ok(
        new
        {
            servicio = "NexusCommerce.Orders",
            estado = "running",
            descripcion = "Microservicio de gestión de pedidos"
        }));

app.MapHealthChecks(
    "/health/live",
    new HealthCheckOptions
    {
        Predicate = _ => false
    });

app.MapHealthChecks(
    "/health/ready",
    new HealthCheckOptions
    {
        Predicate = registro =>
            registro.Tags.Contains("ready")
    });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
