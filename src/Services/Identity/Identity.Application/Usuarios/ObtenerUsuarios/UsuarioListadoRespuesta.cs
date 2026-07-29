namespace Identity.Application.Usuarios.ObtenerUsuarios;

public sealed record UsuarioListadoRespuesta(
    Guid Id,
    string Nombre,
    string Apellidos,
    string NombreCompleto,
    string Correo,
    string Rol,
    string Estado,
    bool EstaActivo,
    DateTime FechaCreacionUtc,
    DateTime? FechaActualizacionUtc);
