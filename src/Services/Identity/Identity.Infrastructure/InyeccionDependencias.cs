using Identity.Infrastructure.Persistencia;
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

        return servicios;
    }
}
