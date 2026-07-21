using Identity.Application.Abstracciones.Seguridad;
using Identity.Domain.Usuarios;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Seguridad;

internal sealed class HashContrasena
    : IHashContrasena
{
    private readonly PasswordHasher<Usuario> _hasher =
        new();

    public string GenerarHash(
        Usuario usuario,
        string contrasena)
    {
        return _hasher.HashPassword(
            usuario,
            contrasena);
    }

    public bool Verificar(
        Usuario usuario,
        string contrasenaHash,
        string contrasena)
    {
        var resultado = _hasher.VerifyHashedPassword(
            usuario,
            contrasenaHash,
            contrasena);

        return resultado is
            PasswordVerificationResult.Success or
            PasswordVerificationResult.SuccessRehashNeeded;
    }
}
