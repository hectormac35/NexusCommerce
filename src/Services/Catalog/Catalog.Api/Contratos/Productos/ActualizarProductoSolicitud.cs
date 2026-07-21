namespace Catalog.Api.Contratos.Productos;

public sealed record ActualizarProductoSolicitud(
    string Nombre,
    string Descripcion,
    decimal Precio,
    string Categoria);
