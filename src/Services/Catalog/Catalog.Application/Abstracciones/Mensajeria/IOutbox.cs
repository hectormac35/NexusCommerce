namespace Catalog.Application.Abstracciones.Mensajeria;

public interface IOutbox
{
    Task AgregarAsync<TEvento>(
        TEvento evento,
        CancellationToken cancellationToken = default)
        where TEvento : IEventoIntegracion;
}
