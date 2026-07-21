using Catalog.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class InyeccionDependencias
{
    public static IServiceCollection AgregarInfraestructuraCatalogo(
        this IServiceCollection servicios,
        IConfiguration configuracion)
    {
        var cadenaConexion = configuracion.GetConnectionString("Catalogo")
            ?? throw new InvalidOperationException(
                "No se ha configurado la conexión 'Catalogo'.");

        servicios.AddDbContext<CatalogoDbContext>(opciones =>
            opciones.UseNpgsql(cadenaConexion));

        return servicios;
    }
}
