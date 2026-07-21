using Identity.Domain.Tokens;
using Identity.Domain.Usuarios;

namespace Identity.Application.Abstracciones.Persistencia;

public interface IRepositorioUsuarios
{
    Task<bool> ExisteCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default);

    Task<Usuario?> ObtenerPorCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default);

    Task<CredencialUsuario?> ObtenerCredencialAsync(
        Guid usuarioId,
        CancellationToken cancellationToken = default);

    void AgregarUsuario(Usuario usuario);

    void AgregarCredencial(CredencialUsuario credencial);

    void AgregarRefreshToken(RefreshToken refreshToken);

    Task<int> GuardarCambiosAsync(
        CancellationToken cancellationToken = default);
}
