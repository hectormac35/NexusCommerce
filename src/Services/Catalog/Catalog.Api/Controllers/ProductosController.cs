using Catalog.Api.Contratos.Productos;
using Catalog.Application.Common.Errors;
using Catalog.Application.Productos;
using Catalog.Application.Productos.Comandos.CrearProducto;
using Catalog.Application.Productos.Comandos.DesactivarProducto;
using Catalog.Application.Productos.Comandos.ActualizarProducto;
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

    [HttpPost]
    [ProducesResponseType<CrearProductoRespuesta>(
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CrearProductoRespuesta>> Crear(
        CrearProductoSolicitud solicitud,
        CancellationToken cancellationToken)
    {
        var comando = new CrearProductoComando(
            solicitud.Nombre,
            solicitud.Descripcion,
            solicitud.Precio,
            solicitud.Stock,
            solicitud.Categoria);

        var resultado = await _sender.Send(
            comando,
            cancellationToken);

        if (resultado.EsFallo)
        {
            var problema = new ProblemDetails
            {
                Status = resultado.Error.Tipo switch
                {
                    TipoError.Conflicto =>
                        StatusCodes.Status409Conflict,

                    TipoError.Validacion =>
                        StatusCodes.Status400BadRequest,

                    _ => StatusCodes.Status500InternalServerError
                },
                Title = resultado.Error.Codigo,
                Detail = resultado.Error.Mensaje,
                Instance = HttpContext.Request.Path
            };

            return StatusCode(problema.Status.Value, problema);
        }

        var respuesta = new CrearProductoRespuesta(
            resultado.Valor);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = resultado.Valor },
            respuesta);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(
        Guid id,
        ActualizarProductoSolicitud solicitud,
        CancellationToken cancellationToken)
    {
        var comando = new ActualizarProductoComando(
            id,
            solicitud.Nombre,
            solicitud.Descripcion,
            solicitud.Precio,
            solicitud.Categoria);

        var resultado = await _sender.Send(
            comando,
            cancellationToken);

        if (resultado.EsExitoso)
        {
            return NoContent();
        }

        var problema = new ProblemDetails
        {
            Status = resultado.Error.Tipo switch
            {
                TipoError.NoEncontrado =>
                    StatusCodes.Status404NotFound,

                TipoError.Conflicto =>
                    StatusCodes.Status409Conflict,

                TipoError.Validacion =>
                    StatusCodes.Status400BadRequest,

                _ => StatusCodes.Status500InternalServerError
            },
            Title = resultado.Error.Codigo,
            Detail = resultado.Error.Mensaje,
            Instance = HttpContext.Request.Path
        };

        return StatusCode(
            problema.Status.Value,
            problema);
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desactivar(
        Guid id,
        CancellationToken cancellationToken)
    {
        var resultado = await _sender.Send(
            new DesactivarProductoComando(id),
            cancellationToken);

        if (resultado.EsExitoso)
        {
            return NoContent();
        }

        var problema = new ProblemDetails
        {
            Status = resultado.Error.Tipo switch
            {
                TipoError.NoEncontrado =>
                    StatusCodes.Status404NotFound,

                _ => StatusCodes.Status500InternalServerError
            },
            Title = resultado.Error.Codigo,
            Detail = resultado.Error.Mensaje,
            Instance = HttpContext.Request.Path
        };

        return StatusCode(
            problema.Status.Value,
            problema);
    }

}
