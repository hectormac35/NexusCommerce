using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Orders.Application;
using Orders.Infrastructure;
using Orders.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

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

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
