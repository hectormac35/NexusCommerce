namespace Catalog.Api.Contratos.Productos;

public sealed record CrearProductoSolicitud(
    string Nombre,
    string Descripcion,
    decimal Precio,
    int Stock,
    string Categoria);
