using Identity.Application.Common.Resultados;

namespace Identity.Application.Autenticacion.CerrarSesion;

public static class ErroresCerrarSesion
{
    public static readonly Error TokenInvalido =
        new(
            "Autenticacion.RefreshTokenInvalido",
            "El refresh token no es válido o ya ha sido revocado.",
            TipoError.NoAutorizado);
}
