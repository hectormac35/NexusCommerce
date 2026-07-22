using System.Text.Json;
using Catalog.Application.Abstracciones.Mensajeria;
using Catalog.Application.Eventos.Integracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catalog.Infrastructure.Persistencia.Outbox;

internal sealed class ProcesadorOutbox(
    IServiceScopeFactory scopeFactory,
    IOptions<OutboxOpciones> opciones,
    ILogger<ProcesadorOutbox> logger)
    : BackgroundService
{
    private readonly OutboxOpciones _opciones =
        opciones.Value;

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        if (!_opciones.Habilitado)
        {
            logger.LogInformation(
                "El procesador Outbox está deshabilitado.");

            return;
        }

        logger.LogInformation(
            "Procesador Outbox iniciado. Intervalo: {Intervalo}s. Lote: {Lote}.",
            _opciones.IntervaloSegundos,
            _opciones.TamanoLote);

        using var temporizador = new PeriodicTimer(
            TimeSpan.FromSeconds(
                _opciones.IntervaloSegundos));

        await ProcesarPendientesAsync(stoppingToken);

        while (await temporizador.WaitForNextTickAsync(
            stoppingToken))
        {
            await ProcesarPendientesAsync(stoppingToken);
        }
    }

    private async Task ProcesarPendientesAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            using var alcance =
                scopeFactory.CreateScope();

            var contexto = alcance.ServiceProvider
                .GetRequiredService<CatalogoDbContext>();

            var busEventos = alcance.ServiceProvider
                .GetRequiredService<IBusEventos>();

            var mensajes = await contexto.MensajesOutbox
                .Where(mensaje =>
                    mensaje.ProcesadoEnUtc == null &&
                    mensaje.Intentos <
                        _opciones.MaximoIntentos)
                .OrderBy(mensaje =>
                    mensaje.OcurridoEnUtc)
                .Take(_opciones.TamanoLote)
                .ToListAsync(cancellationToken);

            if (mensajes.Count == 0)
            {
                return;
            }

            logger.LogInformation(
                "Procesando {Cantidad} mensajes Outbox.",
                mensajes.Count);

            foreach (var mensaje in mensajes)
            {
                await ProcesarMensajeAsync(
                    mensaje,
                    contexto,
                    busEventos,
                    cancellationToken);
            }
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            // El servicio se está deteniendo.
        }
        catch (Exception excepcion)
        {
            logger.LogError(
                excepcion,
                "Error general ejecutando el procesador Outbox.");
        }
    }

    private async Task ProcesarMensajeAsync(
        MensajeOutbox mensaje,
        CatalogoDbContext contexto,
        IBusEventos busEventos,
        CancellationToken cancellationToken)
    {
        try
        {
            switch (mensaje.Tipo)
            {
                case nameof(ProductoCreadoEvento):
                    {
                        var evento =
                            JsonSerializer.Deserialize<
                                ProductoCreadoEvento>(
                                mensaje.Contenido,
                                new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive =
                                        true
                                })
                            ?? throw new JsonException(
                                "No se pudo deserializar ProductoCreadoEvento.");

                        await busEventos.PublicarAsync(
                            evento,
                            cancellationToken);

                        break;
                    }

                default:
                    throw new NotSupportedException(
                        $"El tipo de evento '{mensaje.Tipo}' no está soportado.");
            }

            mensaje.MarcarProcesado(
                DateTime.UtcNow);

            await contexto.SaveChangesAsync(
                cancellationToken);

            logger.LogInformation(
                "Mensaje Outbox {MensajeId} publicado y marcado como procesado.",
                mensaje.Id);
        }
        catch (Exception excepcion)
        {
            mensaje.RegistrarError(
                excepcion.Message,
                DateTime.UtcNow);

            await contexto.SaveChangesAsync(
                cancellationToken);

            logger.LogError(
                excepcion,
                "Error publicando el mensaje Outbox {MensajeId}. Intento {Intento}.",
                mensaje.Id,
                mensaje.Intentos);
        }
    }
}
