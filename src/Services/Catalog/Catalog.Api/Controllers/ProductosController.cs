using Catalog.Api.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/catalogo/productos")]
public sealed class ProductosController : ControllerBase
{
    private static readonly IReadOnlyCollection<Producto> Productos =
    [
        new(
            Guid.Parse("2f8ec43e-5a75-4de4-a8e4-d49c2c270101"),
            "Teclado mecánico",
            "Teclado mecánico compacto con iluminación RGB.",
            89.99m,
            25,
            "Periféricos",
            true),

        new(
            Guid.Parse("2f8ec43e-5a75-4de4-a8e4-d49c2c270102"),
            "Ratón inalámbrico",
            "Ratón ergonómico con conexión inalámbrica.",
            39.95m,
            40,
            "Periféricos",
            true),

        new(
            Guid.Parse("2f8ec43e-5a75-4de4-a8e4-d49c2c270103"),
            "Monitor 27 pulgadas",
            "Monitor IPS con resolución 1440p.",
            299.90m,
            12,
            "Monitores",
            true)
    ];

    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<Producto>>(StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyCollection<Producto>> ObtenerTodos()
    {
        return Ok(Productos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<Producto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Producto> ObtenerPorId(Guid id)
    {
        var producto = Productos.FirstOrDefault(producto => producto.Id == id);

        return producto is null
            ? NotFound()
            : Ok(producto);
    }
}
