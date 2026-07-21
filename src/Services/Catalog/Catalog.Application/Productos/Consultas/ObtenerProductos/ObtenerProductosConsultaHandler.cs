using Catalog.Application.Abstracciones.Persistencia;
using MediatR;

namespace Catalog.Application.Productos.Consultas.ObtenerProductos;

internal sealed class ObtenerProductosConsultaHandler
    : IRequestHandler<
        ObtenerProductosConsulta,
        IReadOnlyCollection<ProductoDto>>
{
    private readonly IConsultaProductos _consultaProductos;

    public ObtenerProductosConsultaHandler(
        IConsultaProductos consultaProductos)
    {
        _consultaProductos = consultaProductos;
    }

    public async Task<IReadOnlyCollection<ProductoDto>> Handle(
        ObtenerProductosConsulta request,
        CancellationToken cancellationToken)
    {
        var productos = await _consultaProductos.ObtenerTodosAsync(
            cancellationToken);

        return productos
            .Select(producto => new ProductoDto(
                producto.Id,
                producto.Nombre,
                producto.Descripcion,
                producto.Precio,
                producto.Stock,
                producto.Categoria,
                producto.EstaActivo,
                producto.TieneStock))
            .ToArray();
    }
}
