using System.Security.Claims;
using Identity.Application.Common.Resultados;
using Identity.Application.Usuarios.ObtenerUsuarioActual;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosController(
    ISender sender)
    : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(
        typeof(UsuarioActualRespuesta),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerUsuarioActual(
        CancellationToken cancellationToken)
    {
        var identificador = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(
                identificador,
                out var usuarioId))
        {
            return Unauthorized();
        }

        var resultado = await sender.Send(
            new ObtenerUsuarioActualConsulta(usuarioId),
            cancellationToken);

        if (resultado.EsExitoso)
        {
            return Ok(resultado.Valor);
        }

        var estado = resultado.Error.Tipo switch
        {
            TipoError.NoEncontrado =>
                StatusCodes.Status404NotFound,

            TipoError.NoAutorizado =>
                StatusCodes.Status401Unauthorized,

            _ =>
                StatusCodes.Status500InternalServerError
        };

        return Problem(
            statusCode: estado,
            title: resultado.Error.Codigo,
            detail: resultado.Error.Mensaje);
    }
}
