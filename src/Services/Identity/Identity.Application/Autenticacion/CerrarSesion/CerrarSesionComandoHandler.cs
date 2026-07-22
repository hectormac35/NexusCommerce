using Identity.Application.Abstracciones.Persistencia;
using Identity.Application.Abstracciones.Seguridad;
using Identity.Application.Common.Resultados;
using MediatR;

namespace Identity.Application.Autenticacion.CerrarSesion;

internal sealed class CerrarSesionComandoHandler
    : IRequestHandler<CerrarSesionComando, Resultado>
{
    private readonly IRepositorioUsuarios _repositorio;
    private readonly IGeneradorTokens _generadorTokens;

    public CerrarSesionComandoHandler(
        IRepositorioUsuarios repositorio,
        IGeneradorTokens generadorTokens)
    {
        _repositorio = repositorio;
        _generadorTokens = generadorTokens;
    }

    public async Task<Resultado> Handle(
        CerrarSesionComando request,
        CancellationToken cancellationToken)
    {
        var tokenHash =
            _generadorTokens.CalcularHashRefreshToken(
                request.RefreshToken);

        var refreshToken =
            await _repositorio.ObtenerRefreshTokenPorHashAsync(
                tokenHash,
                cancellationToken);

        if (refreshToken is null || !refreshToken.EsValido)
        {
            return Resultado.Fallo(
                ErroresCerrarSesion.TokenInvalido);
        }

        refreshToken.Revocar(
            DateTime.UtcNow);

        await _repositorio.GuardarCambiosAsync(
            cancellationToken);

        return Resultado.Exito();
    }
}
