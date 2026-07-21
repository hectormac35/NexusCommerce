using Identity.Application.Common.Resultados;

namespace Identity.Application.Autenticacion.RefrescarSesion;

public static class ErroresRefrescarSesion
{
    public static readonly Error TokenInvalido =
        new(
            "Autenticacion.RefreshTokenInvalido",
            "El refresh token no es válido o ha expirado.",
            TipoError.NoAutorizado);

    public static readonly Error UsuarioInactivo =
        new(
            "Autenticacion.UsuarioInactivo",
            "El usuario no se encuentra activo.",
            TipoError.NoAutorizado);
}
