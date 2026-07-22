namespace Catalog.Application.Abstracciones.Mensajeria;

public interface IEventoIntegracion
{
    Guid EventoId { get; }

    DateTime OcurridoEnUtc { get; }
}
