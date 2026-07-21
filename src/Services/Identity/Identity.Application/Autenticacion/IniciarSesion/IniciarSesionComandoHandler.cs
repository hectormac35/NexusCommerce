using Identity.Application.Abstracciones.Persistencia;
using Identity.Application.Abstracciones.Seguridad;
using Identity.Application.Common.Resultados;
using Identity.Domain.Tokens;
using MediatR;

namespace Identity.Application.Autenticacion.IniciarSesion;

internal sealed class IniciarSesionComandoHandler
    : IRequestHandler<
        IniciarSesionComando,
        Resultado<SesionIniciadaRespuesta>>
{
    private readonly IRepositorioUsuarios _repositorio;
    private readonly IHashContrasena _hashContrasena;
    private readonly IGeneradorTokens _generadorTokens;

    public IniciarSesionComandoHandler(
        IRepositorioUsuarios repositorio,
        IHashContrasena hashContrasena,
        IGeneradorTokens generadorTokens)
    {
        _repositorio = repositorio;
        _hashContrasena = hashContrasena;
        _generadorTokens = generadorTokens;
    }

    public async Task<Resultado<SesionIniciadaRespuesta>> Handle(
        IniciarSesionComando request,
        CancellationToken cancellationToken)
    {
        var correoNormalizado =
            request.Correo.Trim().ToLowerInvariant();

        var usuario = await _repositorio.ObtenerPorCorreoAsync(
            correoNormalizado,
            cancellationToken);

        if (usuario is null)
        {
            return Resultado<SesionIniciadaRespuesta>.Fallo(
                ErroresInicioSesion.CredencialesInvalidas);
        }

        if (!usuario.EstaActivo)
        {
            return Resultado<SesionIniciadaRespuesta>.Fallo(
                ErroresInicioSesion.UsuarioInactivo);
        }

        var credencial =
            await _repositorio.ObtenerCredencialAsync(
                usuario.Id,
                cancellationToken);

        if (credencial is null)
        {
            return Resultado<SesionIniciadaRespuesta>.Fallo(
                ErroresInicioSesion.CredencialesInvalidas);
        }

        var contrasenaCorrecta = _hashContrasena.Verificar(
            usuario,
            credencial.ContrasenaHash,
            request.Contrasena);

        if (!contrasenaCorrecta)
        {
            return Resultado<SesionIniciadaRespuesta>.Fallo(
                ErroresInicioSesion.CredencialesInvalidas);
        }

        var tokens = _generadorTokens.Generar(usuario);

        var refreshToken = new RefreshToken(
            Guid.NewGuid(),
            usuario.Id,
            tokens.RefreshTokenHash,
            DateTime.UtcNow,
            tokens.FechaExpiracionRefreshTokenUtc);

        _repositorio.AgregarRefreshToken(refreshToken);

        await _repositorio.GuardarCambiosAsync(
            cancellationToken);

        var respuesta = new SesionIniciadaRespuesta(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.ExpiraEnSegundos,
            new UsuarioSesionRespuesta(
                usuario.Id,
                usuario.Nombre,
                usuario.Apellidos,
                usuario.Correo,
                usuario.Rol.ToString()));

        return Resultado<SesionIniciadaRespuesta>.Exito(
            respuesta);
    }
}
