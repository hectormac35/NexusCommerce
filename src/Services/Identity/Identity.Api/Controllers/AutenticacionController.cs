using Identity.Api.Contratos.Autenticacion;
using Identity.Application.Autenticacion.Registrar;
using Identity.Application.Common.Resultados;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("api/autenticacion")]
public sealed class AutenticacionController(
    ISender sender)
    : ControllerBase
{
    [HttpPost("registro")]
    [ProducesResponseType(
        typeof(UsuarioRegistradoRespuesta),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registrar(
        RegistrarUsuarioPeticion peticion,
        CancellationToken cancellationToken)
    {
        var comando = new RegistrarUsuarioComando(
            peticion.Nombre,
            peticion.Apellidos,
            peticion.Correo,
            peticion.Contrasena,
            peticion.ConfirmacionContrasena);

        var resultado = await sender.Send(
            comando,
            cancellationToken);

        if (resultado.EsExitoso)
        {
            return Created(
                $"/api/usuarios/{resultado.Valor!.Id}",
                resultado.Valor);
        }

        var estado = resultado.Error.Tipo switch
        {
            TipoError.Conflicto =>
                StatusCodes.Status409Conflict,

            TipoError.Validacion =>
                StatusCodes.Status400BadRequest,

            _ =>
                StatusCodes.Status500InternalServerError
        };

        return Problem(
            statusCode: estado,
            title: resultado.Error.Codigo,
            detail: resultado.Error.Mensaje);
    }
}
