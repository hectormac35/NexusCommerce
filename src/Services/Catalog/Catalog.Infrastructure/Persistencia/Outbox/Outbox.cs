using System.Text.Json;
using Catalog.Application.Abstracciones.Mensajeria;

namespace Catalog.Infrastructure.Persistencia.Outbox;

internal sealed class Outbox(
    CatalogoDbContext contexto)
    : IOutbox
{
    public async Task AgregarAsync<TEvento>(
        TEvento evento,
        CancellationToken cancellationToken = default)
        where TEvento : IEventoIntegracion
    {
        ArgumentNullException.ThrowIfNull(evento);

        var contenido = JsonSerializer.Serialize(
            evento,
            evento.GetType());

        var mensaje = new MensajeOutbox(
            evento.EventoId,
            evento.OcurridoEnUtc,
            evento.GetType().Name,
            contenido);

        await contexto.MensajesOutbox.AddAsync(
            mensaje,
            cancellationToken);
    }
}
