namespace Catalog.Api.Modelos;

public sealed record Producto(
    Guid Id,
    string Nombre,
    string Descripcion,
    decimal Precio,
    int Stock,
    string Categoria,
    bool EstaActivo);
