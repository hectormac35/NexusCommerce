using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Infrastructure.Mensajeria.RabbitMq;

namespace Notification.Infrastructure;

public static class InyeccionDependencias
{
    public static IServiceCollection
        AgregarInfraestructuraNotificaciones(
            this IServiceCollection servicios,
            IConfiguration configuracion)
    {
        servicios
            .AddOptions<RabbitMqOpciones>()
            .Bind(
                configuracion.GetSection(
                    RabbitMqOpciones.Seccion))
            .ValidateOnStart();

        servicios.AddHostedService<
            ConsumidorProductoCreado>();

        return servicios;
    }
}
