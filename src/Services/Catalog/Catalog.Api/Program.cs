using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Persistencia;
using Catalog.Infrastructure.Persistencia.Inicializacion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AgregarAplicacionCatalogo();

builder.Services.AgregarInfraestructuraCatalogo(
    builder.Configuration);

var app = builder.Build();

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

app.MapControllers();

app.Run();
