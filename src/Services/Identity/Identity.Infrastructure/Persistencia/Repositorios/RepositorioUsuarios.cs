using Identity.Application.Abstracciones.Persistencia;
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

    public void AgregarUsuario(Usuario usuario)
    {
        contexto.Usuarios.Add(usuario);
    }

    public void AgregarCredencial(
        CredencialUsuario credencial)
    {
        contexto.CredencialesUsuarios.Add(credencial);
    }

    public Task<int> GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        return contexto.SaveChangesAsync(
            cancellationToken);
    }
}
