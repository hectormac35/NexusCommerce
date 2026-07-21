using Catalog.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application;

public static class InyeccionDependencias
{
    public static IServiceCollection AgregarAplicacionCatalogo(
        this IServiceCollection servicios)
    {
        var ensamblado = typeof(InyeccionDependencias).Assembly;

        servicios.AddMediatR(configuracion =>
            configuracion.RegisterServicesFromAssembly(ensamblado));

        servicios.AddValidatorsFromAssembly(ensamblado);

        servicios.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ComportamientoValidacion<,>));

        return servicios;
    }
}
