using Catalog.Application.Productos;
using Catalog.Application.Productos.Consultas.ObtenerProductoPorId;
using Catalog.Application.Productos.Consultas.ObtenerProductos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/catalogo/productos")]
public sealed class ProductosController : ControllerBase
{
    private readonly ISender _sender;

    public ProductosController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<ProductoDto>>(
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ProductoDto>>>
        ObtenerTodos(CancellationToken cancellationToken)
    {
        var productos = await _sender.Send(
            new ObtenerProductosConsulta(),
            cancellationToken);

        return Ok(productos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<ProductoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> ObtenerPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var producto = await _sender.Send(
            new ObtenerProductoPorIdConsulta(id),
            cancellationToken);

        return producto is null
            ? NotFound()
            : Ok(producto);
    }
}
