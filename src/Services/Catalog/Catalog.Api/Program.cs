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

app.MapControllers();

app.Run();
