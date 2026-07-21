var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddOpenApi();

var app = builder.Build();

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

app.MapReverseProxy();

app.Run();
