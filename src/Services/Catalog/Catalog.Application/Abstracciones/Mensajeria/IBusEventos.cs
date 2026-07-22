namespace Catalog.Application.Abstracciones.Mensajeria;

public interface IBusEventos
{
    Task PublicarAsync<TEvento>(
        TEvento evento,
        CancellationToken cancellationToken = default)
        where TEvento : IEventoIntegracion;
}
