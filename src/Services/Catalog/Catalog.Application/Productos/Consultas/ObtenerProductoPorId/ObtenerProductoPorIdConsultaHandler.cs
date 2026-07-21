using Catalog.Application.Abstracciones.Persistencia;
using MediatR;

namespace Catalog.Application.Productos.Consultas.ObtenerProductoPorId;

internal sealed class ObtenerProductoPorIdConsultaHandler
    : IRequestHandler<ObtenerProductoPorIdConsulta, ProductoDto?>
{
    private readonly IConsultaProductos _consultaProductos;

    public ObtenerProductoPorIdConsultaHandler(
        IConsultaProductos consultaProductos)
    {
        _consultaProductos = consultaProductos;
    }

    public async Task<ProductoDto?> Handle(
        ObtenerProductoPorIdConsulta request,
        CancellationToken cancellationToken)
    {
        var producto = await _consultaProductos.ObtenerPorIdAsync(
            request.Id,
            cancellationToken);

        if (producto is null)
        {
            return null;
        }

        return new ProductoDto(
            producto.Id,
            producto.Nombre,
            producto.Descripcion,
            producto.Precio,
            producto.Stock,
            producto.Categoria,
            producto.EstaActivo,
            producto.TieneStock);
    }
}
