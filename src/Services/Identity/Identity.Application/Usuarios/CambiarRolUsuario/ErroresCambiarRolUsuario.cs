using Identity.Application.Common.Resultados;

namespace Identity.Application.Usuarios.CambiarRolUsuario;

internal static class ErroresCambiarRolUsuario
{
    public static readonly Error UsuarioNoEncontrado =
        new(
            "Usuarios.NoEncontrado",
            "El usuario indicado no existe.",
            TipoError.NoEncontrado);
}
