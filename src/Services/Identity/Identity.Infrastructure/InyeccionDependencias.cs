using Identity.Application.Abstracciones.Persistencia;
using Identity.Application.Abstracciones.Seguridad;
using Identity.Infrastructure.Persistencia;
using Identity.Infrastructure.Persistencia.Repositorios;
using Identity.Infrastructure.Seguridad;
using Identity.Infrastructure.Seguridad.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class InyeccionDependencias
{
    public static IServiceCollection AgregarInfraestructuraIdentidad(
        this IServiceCollection servicios,
        IConfiguration configuracion)
    {
        var cadenaConexion = configuracion
            .GetConnectionString("Identidad")
            ?? throw new InvalidOperationException(
                "No se ha configurado la conexión 'Identidad'.");

        servicios.AddDbContext<IdentidadDbContext>(
            opciones =>
                opciones.UseNpgsql(cadenaConexion));

        servicios.Configure<JwtOpciones>(
            configuracion.GetSection(
                JwtOpciones.Seccion));

        servicios.AddScoped<
            IRepositorioUsuarios,
            RepositorioUsuarios>();

        servicios.AddSingleton<
            IHashContrasena,
            HashContrasena>();

        servicios.AddSingleton<
            IGeneradorTokens,
            GeneradorTokens>();

        return servicios;
    }
}
