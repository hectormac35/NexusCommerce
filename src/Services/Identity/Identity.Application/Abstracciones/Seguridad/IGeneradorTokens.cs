using Identity.Domain.Usuarios;

namespace Identity.Application.Abstracciones.Seguridad;

public interface IGeneradorTokens
{
    TokenGenerado Generar(Usuario usuario);

    string CalcularHashRefreshToken(
        string refreshToken);
}

public sealed record TokenGenerado(
    string AccessToken,
    string RefreshToken,
    string RefreshTokenHash,
    int ExpiraEnSegundos,
    DateTime FechaExpiracionRefreshTokenUtc);
