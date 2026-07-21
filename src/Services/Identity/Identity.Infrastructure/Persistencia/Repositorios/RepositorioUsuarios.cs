using Identity.Application.Abstracciones.Persistencia;
using Identity.Domain.Tokens;
using Identity.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistencia.Repositorios;

internal sealed class RepositorioUsuarios(
    IdentidadDbContext contexto)
    : IRepositorioUsuarios
{
    public Task<bool> ExisteCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        return contexto.Usuarios.AnyAsync(
            usuario => usuario.Correo == correo,
            cancellationToken);
    }

    public Task<Usuario?> ObtenerPorCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        return contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(
                usuario => usuario.Correo == correo,
                cancellationToken);
    }

    public Task<CredencialUsuario?> ObtenerCredencialAsync(
        Guid usuarioId,
        CancellationToken cancellationToken = default)
    {
        return contexto.CredencialesUsuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(
                credencial =>
                    credencial.UsuarioId == usuarioId,
                cancellationToken);
    }

    public void AgregarUsuario(Usuario usuario)
    {
        contexto.Usuarios.Add(usuario);
    }

    public void AgregarCredencial(
        CredencialUsuario credencial)
    {
        contexto.CredencialesUsuarios.Add(credencial);
    }

    public void AgregarRefreshToken(
        RefreshToken refreshToken)
    {
        contexto.RefreshTokens.Add(refreshToken);
    }

    public Task<int> GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        return contexto.SaveChangesAsync(
            cancellationToken);
    }
}
