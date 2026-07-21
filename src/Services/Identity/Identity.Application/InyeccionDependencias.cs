using FluentValidation;
using Identity.Application.Common.Validacion;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class InyeccionDependencias
{
    public static IServiceCollection AgregarAplicacionIdentidad(
        this IServiceCollection servicios)
    {
        var ensamblado = typeof(InyeccionDependencias).Assembly;

        servicios.AddMediatR(
            configuracion =>
                configuracion.RegisterServicesFromAssembly(
                    ensamblado));

        servicios.AddValidatorsFromAssembly(ensamblado);

        servicios.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ComportamientoValidacion<,>));

        return servicios;
    }
}
