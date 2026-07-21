using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application;

public static class InyeccionDependencias
{
    public static IServiceCollection AgregarAplicacionCatalogo(
        this IServiceCollection servicios)
    {
        servicios.AddMediatR(configuracion =>
            configuracion.RegisterServicesFromAssembly(
                typeof(InyeccionDependencias).Assembly));

        return servicios;
    }
}
