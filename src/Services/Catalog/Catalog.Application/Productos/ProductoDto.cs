namespace Catalog.Application.Productos;

public sealed record ProductoDto(
    Guid Id,
    string Nombre,
    string Descripcion,
    decimal Precio,
    int Stock,
    string Categoria,
    bool EstaActivo,
    bool TieneStock);
