using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Catalog.Api.ManejoErrores;
using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Persistencia;
using Catalog.Infrastructure.Persistencia.Inicializacion;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorExcepcionesGlobal>();

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(recurso =>
        recurso.AddService(
            serviceName: "NexusCommerce.Catalog",
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
            .AddSource("NexusCommerce.Catalog")
            .AddOtlpExporter(opciones =>
            {
                opciones.Endpoint = new Uri(
                    builder.Configuration[
                        "OpenTelemetry:OtlpEndpoint"]
                    ?? "http://localhost:4317");
            });
    });

builder.Services.AgregarAplicacionCatalogo();

builder.Services.AgregarInfraestructuraCatalogo(
    builder.Configuration);

var cadenaConexion = builder.Configuration
    .GetConnectionString("Catalogo")
    ?? throw new InvalidOperationException(
        "No se ha configurado la conexión 'Catalogo'.");

builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        cadenaConexion,
        name: "postgres-catalogo",
        tags: ["ready"]);


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

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var alcance = app.Services.CreateScope())
{
    var contexto = alcance.ServiceProvider
        .GetRequiredService<CatalogoDbContext>();

    await InicializadorCatalogo.InicializarAsync(contexto);
}

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
