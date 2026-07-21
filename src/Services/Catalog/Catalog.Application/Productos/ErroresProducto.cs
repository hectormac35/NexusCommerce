using Catalog.Application.Common.Errors;

namespace Catalog.Application.Productos;

public static class ErroresProducto
{
    public static Error NoEncontrado(Guid id)
    {
        return Error.NoEncontrado(
            "Productos.NoEncontrado",
            $"No se encontró el producto con identificador '{id}'.");
    }

    public static readonly Error NombreDuplicado = Error.Conflicto(
        "Productos.NombreDuplicado",
        "Ya existe un producto con el mismo nombre.");

    public static readonly Error PrecioInvalido = Error.Validacion(
        "Productos.PrecioInvalido",
        "El precio del producto debe ser mayor que cero.");

    public static readonly Error StockInvalido = Error.Validacion(
        "Productos.StockInvalido",
        "El stock del producto no puede ser negativo.");
}
