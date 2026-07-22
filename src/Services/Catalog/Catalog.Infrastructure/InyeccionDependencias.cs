using Catalog.Application.Abstracciones.Mensajeria;
using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Infrastructure.Mensajeria.RabbitMq;
using Catalog.Infrastructure.Persistencia;
using Catalog.Infrastructure.Persistencia.Consultas;
using Catalog.Infrastructure.Persistencia.Outbox;
using Catalog.Infrastructure.Persistencia.Repositorios;
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
        var cadenaConexion = configuracion
            .GetConnectionString("Catalogo")
            ?? throw new InvalidOperationException(
                "No se ha configurado la conexión 'Catalogo'.");

        servicios.AddDbContext<CatalogoDbContext>(opciones =>
            opciones.UseNpgsql(cadenaConexion));

        servicios.AddScoped<
            IConsultaProductos,
            ConsultaProductos>();

        servicios.AddScoped<
            IRepositorioProductos,
            RepositorioProductos>();

        servicios.AddScoped<
            IOutbox,
            Outbox>();

        servicios
            .AddOptions<RabbitMqOpciones>()
            .Bind(
                configuracion.GetSection(
                    RabbitMqOpciones.Seccion))
            .Validate(
                opciones =>
                    !string.IsNullOrWhiteSpace(
                        opciones.Host),
                "RabbitMQ requiere un host.")
            .Validate(
                opciones =>
                    !string.IsNullOrWhiteSpace(
                        opciones.Usuario),
                "RabbitMQ requiere un usuario.")
            .Validate(
                opciones =>
                    !string.IsNullOrWhiteSpace(
                        opciones.Contrasena),
                "RabbitMQ requiere una contraseña.")
            .ValidateOnStart();

        servicios.AddSingleton<
            IBusEventos,
            RabbitMqBusEventos>();

        servicios
            .AddOptions<OutboxOpciones>()
            .Bind(
                configuracion.GetSection(
                    OutboxOpciones.Seccion))
            .Validate(
                opciones =>
                    opciones.IntervaloSegundos > 0,
                "El intervalo de Outbox debe ser mayor que cero.")
            .Validate(
                opciones =>
                    opciones.TamanoLote > 0,
                "El tamaño del lote Outbox debe ser mayor que cero.")
            .Validate(
                opciones =>
                    opciones.MaximoIntentos > 0,
                "El máximo de intentos debe ser mayor que cero.")
            .ValidateOnStart();

        servicios.AddHostedService<
            ProcesadorOutbox>();

        return servicios;
    }
}
