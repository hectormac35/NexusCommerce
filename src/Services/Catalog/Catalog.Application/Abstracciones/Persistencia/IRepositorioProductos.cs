using Catalog.Domain.Productos;

namespace Catalog.Application.Abstracciones.Persistencia;

public interface IRepositorioProductos
{
    Task<bool> ExisteNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default);

    Task AgregarAsync(
        Producto producto,
        CancellationToken cancellationToken = default);

    Task<int> GuardarCambiosAsync(
        CancellationToken cancellationToken = default);
}
