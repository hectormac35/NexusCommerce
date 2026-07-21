using Catalog.Domain.Productos;

namespace Catalog.Application.Abstracciones.Persistencia;

public interface IRepositorioProductos
{
    Task<Producto?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExisteNombreAsync(
        string nombre,
        Guid? productoExcluidoId = null,
        CancellationToken cancellationToken = default);

    Task AgregarAsync(
        Producto producto,
        CancellationToken cancellationToken = default);

    Task<int> GuardarCambiosAsync(
        CancellationToken cancellationToken = default);
}
