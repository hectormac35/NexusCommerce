using Identity.Application.Abstracciones.Persistencia;
using Identity.Application.Abstracciones.Seguridad;
using Identity.Application.Autenticacion.IniciarSesion;
using Identity.Application.Common.Resultados;
using Identity.Domain.Tokens;
using MediatR;

namespace Identity.Application.Autenticacion.RefrescarSesion;

internal sealed class RefrescarSesionComandoHandler
    : IRequestHandler<
        RefrescarSesionComando,
        Resultado<SesionIniciadaRespuesta>>
{
    private readonly IRepositorioUsuarios _repositorio;
    private readonly IGeneradorTokens _generadorTokens;

    public RefrescarSesionComandoHandler(
        IRepositorioUsuarios repositorio,
        IGeneradorTokens generadorTokens)
    {
        _repositorio = repositorio;
        _generadorTokens = generadorTokens;
    }

    public async Task<Resultado<SesionIniciadaRespuesta>> Handle(
        RefrescarSesionComando request,
        CancellationToken cancellationToken)
    {
        var tokenHash =
            _generadorTokens.CalcularHashRefreshToken(
                request.RefreshToken);

        var tokenActual =
            await _repositorio.ObtenerRefreshTokenPorHashAsync(
                tokenHash,
                cancellationToken);

        if (tokenActual is null || !tokenActual.EsValido)
        {
            return Resultado<SesionIniciadaRespuesta>.Fallo(
                ErroresRefrescarSesion.TokenInvalido);
        }

        var usuario = await _repositorio.ObtenerPorIdAsync(
            tokenActual.UsuarioId,
            cancellationToken);

        if (usuario is null || !usuario.EstaActivo)
        {
            return Resultado<SesionIniciadaRespuesta>.Fallo(
                ErroresRefrescarSesion.UsuarioInactivo);
        }

        var nuevosTokens =
            _generadorTokens.Generar(usuario);

        var ahora = DateTime.UtcNow;

        tokenActual.Revocar(
            ahora,
            nuevosTokens.RefreshTokenHash);

        var nuevoRefreshToken = new RefreshToken(
            Guid.NewGuid(),
            usuario.Id,
            nuevosTokens.RefreshTokenHash,
            ahora,
            nuevosTokens.FechaExpiracionRefreshTokenUtc);

        _repositorio.AgregarRefreshToken(
            nuevoRefreshToken);

        await _repositorio.GuardarCambiosAsync(
            cancellationToken);

        var respuesta = new SesionIniciadaRespuesta(
            nuevosTokens.AccessToken,
            nuevosTokens.RefreshToken,
            nuevosTokens.ExpiraEnSegundos,
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
