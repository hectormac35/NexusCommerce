using Identity.Infrastructure;
using Identity.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AgregarInfraestructuraIdentidad(
    builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var alcance = app.Services.CreateScope())
{
    var contexto = alcance.ServiceProvider
        .GetRequiredService<IdentidadDbContext>();

    await contexto.Database.MigrateAsync();
}

app.MapGet("/", () => Results.Ok(new
{
    servicio = "Identity.Api",
    estado = "Ejecutándose"
}));

app.MapControllers();

app.Run();

public partial class Program;
