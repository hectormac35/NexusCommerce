using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Identity.Api.ManejoErrores;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<
    ManejadorExcepcionesGlobal>();

builder.Services.AgregarAplicacionIdentidad();

builder.Services.AgregarInfraestructuraIdentidad(
    builder.Configuration);


var jwtClave = builder.Configuration["Jwt:Clave"]
    ?? throw new InvalidOperationException(
        "No se ha configurado la clave JWT.");

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer =
                    builder.Configuration["Jwt:Emisor"],

                ValidateAudience = true,
                ValidAudience =
                    builder.Configuration["Jwt:Audiencia"],

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

app.UseAuthentication();
app.UseAuthorization();

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
