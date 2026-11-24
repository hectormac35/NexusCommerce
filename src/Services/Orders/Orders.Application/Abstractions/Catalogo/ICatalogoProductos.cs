namespace Orders.Application.Abstractions.Catalogo;

/// <summary>
/// Consulta productos en el microservicio de catálogo.
/// El precio y el nombre de cada línea salen de aquí, nunca del cliente.
/// </summary>
public interface ICatalogoProductos
{
    /// <returns>El producto, o <c>null</c> si no existe.</returns>
    /// <exception cref="CatalogoNoDisponibleException">Si el catálogo no responde.</exception>
    Task<ProductoCatalogo?> ObtenerAsync(
        Guid productoId,
        CancellationToken cancellationToken = default);
}

public sealed record ProductoCatalogo(
    Guid Id,
    string Nombre,
    decimal Precio,
    int Stock,
    bool EstaActivo);

public sealed class CatalogoNoDisponibleException(string mensaje, Exception? causa = null)
    : Exception(mensaje, causa);
