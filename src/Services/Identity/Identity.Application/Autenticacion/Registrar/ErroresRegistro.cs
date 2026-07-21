using Identity.Application.Common.Resultados;

namespace Identity.Application.Autenticacion.Registrar;

public static class ErroresRegistro
{
    public static Error CorreoDuplicado(string correo) =>
        new(
            "Usuarios.CorreoDuplicado",
            $"Ya existe un usuario registrado con el correo '{correo}'.",
            TipoError.Conflicto);
}
