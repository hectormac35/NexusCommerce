using Identity.Domain.Common;

namespace Identity.Domain.Usuarios;

public sealed class CredencialUsuario
{
    private CredencialUsuario()
    {
    }

    public CredencialUsuario(
        Guid id,
        Guid usuarioId,
        string contrasenaHash)
    {
        ValidarId(id);
        ValidarUsuarioId(usuarioId);
        ValidarContrasenaHash(contrasenaHash);

        Id = id;
        UsuarioId = usuarioId;
        ContrasenaHash = contrasenaHash;
        FechaCreacionUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid UsuarioId { get; private set; }

    public string ContrasenaHash { get; private set; } =
        string.Empty;

    public DateTime FechaCreacionUtc { get; private set; }

    public DateTime? FechaActualizacionUtc { get; private set; }

    public void ActualizarContrasenaHash(
        string nuevaContrasenaHash)
    {
        ValidarContrasenaHash(nuevaContrasenaHash);

        if (ContrasenaHash == nuevaContrasenaHash)
        {
            throw new ExcepcionDominio(
                "La nueva contraseña cifrada no puede ser igual a la actual.");
        }

        ContrasenaHash = nuevaContrasenaHash;
        FechaActualizacionUtc = DateTime.UtcNow;
    }

    private static void ValidarId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ExcepcionDominio(
                "El identificador de la credencial es obligatorio.");
        }
    }

    private static void ValidarUsuarioId(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
        {
            throw new ExcepcionDominio(
                "El identificador del usuario es obligatorio.");
        }
    }

    private static void ValidarContrasenaHash(
        string contrasenaHash)
    {
        if (string.IsNullOrWhiteSpace(contrasenaHash))
        {
            throw new ExcepcionDominio(
                "La contraseña cifrada es obligatoria.");
        }

        if (contrasenaHash.Trim().Length > 1000)
        {
            throw new ExcepcionDominio(
                "La contraseña cifrada supera la longitud permitida.");
        }
    }
}
