using Catalog.Domain.Productos;

namespace Catalog.Application.Abstracciones.Persistencia;

public interface IConsultaProductos
{
    Task<IReadOnlyCollection<Producto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    Task<Producto?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
