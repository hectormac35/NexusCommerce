using System.Security.Claims;
using Identity.Api.Contratos.Usuarios;
using Identity.Application.Common.Resultados;
using Identity.Application.Usuarios.CambiarRolUsuario;
using Identity.Application.Usuarios.ObtenerUsuarioActual;
using Identity.Application.Usuarios.ObtenerUsuarios;
using Identity.Domain.Usuarios;
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
    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<UsuarioListadoRespuesta>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<
        IReadOnlyList<UsuarioListadoRespuesta>>> ObtenerUsuarios(
        CancellationToken cancellationToken)
    {
        var usuarios = await sender.Send(
            new ObtenerUsuariosConsulta(),
            cancellationToken);

        return Ok(usuarios);
    }

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

    [Authorize(Roles = nameof(RolUsuario.Administrador))]
    [HttpPatch("{usuarioId:guid}/rol")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarRolUsuario(
        Guid usuarioId,
        CambiarRolUsuarioPeticion peticion,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<RolUsuario>(
                peticion.NuevoRol,
                ignoreCase: true,
                out var nuevoRol)
            || !Enum.IsDefined(nuevoRol))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Usuarios.RolNoValido",
                detail:
                    "El rol debe ser Cliente, Empleado o Administrador.");
        }

        var resultado = await sender.Send(
            new CambiarRolUsuarioComando(
                usuarioId,
                nuevoRol),
            cancellationToken);

        if (resultado.EsExitoso)
        {
            return NoContent();
        }

        var estado = resultado.Error.Tipo switch
        {
            TipoError.NoEncontrado =>
                StatusCodes.Status404NotFound,

            TipoError.Validacion =>
                StatusCodes.Status400BadRequest,

            TipoError.NoAutorizado =>
                StatusCodes.Status403Forbidden,

            _ =>
                StatusCodes.Status500InternalServerError
        };

        return Problem(
            statusCode: estado,
            title: resultado.Error.Codigo,
            detail: resultado.Error.Mensaje);
    }
}
