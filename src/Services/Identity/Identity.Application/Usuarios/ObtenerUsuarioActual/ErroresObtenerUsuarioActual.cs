using Identity.Application.Common.Resultados;

namespace Identity.Application.Usuarios.ObtenerUsuarioActual;

public static class ErroresObtenerUsuarioActual
{
    public static readonly Error UsuarioNoEncontrado =
        new(
            "Usuarios.NoEncontrado",
            "No se ha encontrado el usuario autenticado.",
            TipoError.NoEncontrado);

    public static readonly Error UsuarioInactivo =
        new(
            "Usuarios.Inactivo",
            "El usuario autenticado no se encuentra activo.",
            TipoError.NoAutorizado);
}
