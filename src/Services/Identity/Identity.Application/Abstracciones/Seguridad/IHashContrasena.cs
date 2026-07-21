using Identity.Domain.Usuarios;

namespace Identity.Application.Abstracciones.Seguridad;

public interface IHashContrasena
{
    string GenerarHash(
        Usuario usuario,
        string contrasena);

    bool Verificar(
        Usuario usuario,
        string contrasenaHash,
        string contrasena);
}
