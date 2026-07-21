using Identity.Application.Abstracciones.Persistencia;
using Identity.Application.Abstracciones.Seguridad;
using Identity.Application.Common.Resultados;
using Identity.Domain.Usuarios;
using MediatR;

namespace Identity.Application.Autenticacion.Registrar;

internal sealed class RegistrarUsuarioComandoHandler
    : IRequestHandler<
        RegistrarUsuarioComando,
        Resultado<UsuarioRegistradoRespuesta>>
{
    private readonly IRepositorioUsuarios _repositorio;
    private readonly IHashContrasena _hashContrasena;

    public RegistrarUsuarioComandoHandler(
        IRepositorioUsuarios repositorio,
        IHashContrasena hashContrasena)
    {
        _repositorio = repositorio;
        _hashContrasena = hashContrasena;
    }

    public async Task<Resultado<UsuarioRegistradoRespuesta>> Handle(
        RegistrarUsuarioComando request,
        CancellationToken cancellationToken)
    {
        var correoNormalizado =
            request.Correo.Trim().ToLowerInvariant();

        var correoExistente =
            await _repositorio.ExisteCorreoAsync(
                correoNormalizado,
                cancellationToken);

        if (correoExistente)
        {
            return Resultado<UsuarioRegistradoRespuesta>.Fallo(
                ErroresRegistro.CorreoDuplicado(
                    correoNormalizado));
        }

        var usuario = new Usuario(
            Guid.NewGuid(),
            request.Nombre,
            request.Apellidos,
            correoNormalizado);

        var contrasenaHash =
            _hashContrasena.GenerarHash(
                usuario,
                request.Contrasena);

        var credencial = new CredencialUsuario(
            Guid.NewGuid(),
            usuario.Id,
            contrasenaHash);

        _repositorio.AgregarUsuario(usuario);
        _repositorio.AgregarCredencial(credencial);

        await _repositorio.GuardarCambiosAsync(
            cancellationToken);

        var respuesta = new UsuarioRegistradoRespuesta(
            usuario.Id,
            usuario.Nombre,
            usuario.Apellidos,
            usuario.Correo,
            usuario.Rol.ToString());

        return Resultado<UsuarioRegistradoRespuesta>.Exito(
            respuesta);
    }
}
