using Identity.Application.Common.Resultados;

namespace Identity.Application.Autenticacion.IniciarSesion;

public static class ErroresInicioSesion
{
    public static readonly Error CredencialesInvalidas =
        new(
            "Autenticacion.CredencialesInvalidas",
            "El correo electrónico o la contraseña no son correctos.",
            TipoError.NoAutorizado);

    public static readonly Error UsuarioInactivo =
        new(
            "Autenticacion.UsuarioInactivo",
            "El usuario no se encuentra activo.",
            TipoError.NoAutorizado);
}
