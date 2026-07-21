using System.Net.Mail;
using Identity.Domain.Common;

namespace Identity.Domain.Usuarios;

public sealed class Usuario
{
    private Usuario()
    {
    }

    public Usuario(
        Guid id,
        string nombre,
        string apellidos,
        string correo,
        RolUsuario rol = RolUsuario.Cliente)
    {
        ValidarId(id);
        ValidarNombre(nombre);
        ValidarApellidos(apellidos);
        ValidarCorreo(correo);
        ValidarRol(rol);

        Id = id;
        Nombre = nombre.Trim();
        Apellidos = apellidos.Trim();
        Correo = NormalizarCorreo(correo);
        Rol = rol;
        Estado = EstadoUsuario.Activo;
        FechaCreacionUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public string Apellidos { get; private set; } = string.Empty;

    public string Correo { get; private set; } = string.Empty;

    public RolUsuario Rol { get; private set; }

    public EstadoUsuario Estado { get; private set; }

    public DateTime FechaCreacionUtc { get; private set; }

    public DateTime? FechaActualizacionUtc { get; private set; }

    public string NombreCompleto =>
        $"{Nombre} {Apellidos}";

    public bool EstaActivo =>
        Estado == EstadoUsuario.Activo;

    public void ActualizarPerfil(
        string nombre,
        string apellidos)
    {
        ValidarNombre(nombre);
        ValidarApellidos(apellidos);

        Nombre = nombre.Trim();
        Apellidos = apellidos.Trim();

        MarcarComoActualizado();
    }

    public void CambiarCorreo(string correo)
    {
        ValidarCorreo(correo);

        Correo = NormalizarCorreo(correo);

        MarcarComoActualizado();
    }

    public void CambiarRol(RolUsuario nuevoRol)
    {
        ValidarRol(nuevoRol);

        if (Rol == nuevoRol)
        {
            return;
        }

        Rol = nuevoRol;

        MarcarComoActualizado();
    }

    public void Bloquear()
    {
        if (Estado == EstadoUsuario.Desactivado)
        {
            throw new ExcepcionDominio(
                "No se puede bloquear un usuario desactivado.");
        }

        if (Estado == EstadoUsuario.Bloqueado)
        {
            return;
        }

        Estado = EstadoUsuario.Bloqueado;

        MarcarComoActualizado();
    }

    public void Desbloquear()
    {
        if (Estado != EstadoUsuario.Bloqueado)
        {
            throw new ExcepcionDominio(
                "Solo se puede desbloquear un usuario bloqueado.");
        }

        Estado = EstadoUsuario.Activo;

        MarcarComoActualizado();
    }

    public void Desactivar()
    {
        if (Estado == EstadoUsuario.Desactivado)
        {
            return;
        }

        Estado = EstadoUsuario.Desactivado;

        MarcarComoActualizado();
    }

    private void MarcarComoActualizado()
    {
        FechaActualizacionUtc = DateTime.UtcNow;
    }

    private static void ValidarId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ExcepcionDominio(
                "El identificador del usuario es obligatorio.");
        }
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ExcepcionDominio(
                "El nombre es obligatorio.");
        }

        if (nombre.Trim().Length > 100)
        {
            throw new ExcepcionDominio(
                "El nombre no puede superar los 100 caracteres.");
        }
    }

    private static void ValidarApellidos(string apellidos)
    {
        if (string.IsNullOrWhiteSpace(apellidos))
        {
            throw new ExcepcionDominio(
                "Los apellidos son obligatorios.");
        }

        if (apellidos.Trim().Length > 150)
        {
            throw new ExcepcionDominio(
                "Los apellidos no pueden superar los 150 caracteres.");
        }
    }

    private static void ValidarCorreo(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
        {
            throw new ExcepcionDominio(
                "El correo electrónico es obligatorio.");
        }

        try
        {
            var correoNormalizado = correo.Trim();
            var direccion = new MailAddress(correoNormalizado);

            if (direccion.Address != correoNormalizado)
            {
                throw new ExcepcionDominio(
                    "El correo electrónico no tiene un formato válido.");
            }
        }
        catch (FormatException)
        {
            throw new ExcepcionDominio(
                "El correo electrónico no tiene un formato válido.");
        }
    }

    private static string NormalizarCorreo(string correo)
    {
        return correo.Trim().ToLowerInvariant();
    }

    private static void ValidarRol(RolUsuario rol)
    {
        if (!Enum.IsDefined(rol))
        {
            throw new ExcepcionDominio(
                "El rol del usuario no es válido.");
        }
    }
}
