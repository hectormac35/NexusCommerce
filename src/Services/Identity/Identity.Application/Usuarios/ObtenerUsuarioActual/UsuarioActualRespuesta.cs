namespace Identity.Application.Usuarios.ObtenerUsuarioActual;

public sealed record UsuarioActualRespuesta(
    Guid Id,
    string Nombre,
    string Apellidos,
    string Correo,
    string Rol);
