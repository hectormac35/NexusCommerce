using Identity.Domain.Usuarios;

namespace Identity.Application.Abstracciones.Persistencia;

public interface IRepositorioUsuarios
{
    Task<bool> ExisteCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default);

    void AgregarUsuario(Usuario usuario);

    void AgregarCredencial(CredencialUsuario credencial);

    Task<int> GuardarCambiosAsync(
        CancellationToken cancellationToken = default);
}
