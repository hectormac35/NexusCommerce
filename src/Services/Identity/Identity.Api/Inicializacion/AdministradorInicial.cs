using Identity.Application.Abstracciones.Persistencia;
using Identity.Application.Abstracciones.Seguridad;
using Identity.Domain.Usuarios;

namespace Identity.Api.Inicializacion;

/// <summary>
/// Crea un administrador al arrancar si se configura
/// <c>AdministradorInicial:Correo</c> y <c>AdministradorInicial:Contrasena</c>.
/// Sin él no habría forma de gestionar el catálogo, los pedidos ni los roles,
/// porque el registro público siempre crea usuarios con rol Cliente.
/// </summary>
internal static class AdministradorInicial
{
    public static async Task CrearSiNoExisteAsync(
        IServiceProvider servicios,
        IConfiguration configuracion,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        string? correo = configuracion["AdministradorInicial:Correo"];
        string? contrasena = configuracion["AdministradorInicial:Contrasena"];

        if (string.IsNullOrWhiteSpace(correo)
            || string.IsNullOrWhiteSpace(contrasena))
        {
            return;
        }

        var repositorio = servicios.GetRequiredService<IRepositorioUsuarios>();
        string correoNormalizado = correo.Trim().ToLowerInvariant();

        if (await repositorio.ExisteCorreoAsync(correoNormalizado, cancellationToken))
        {
            return;
        }

        var hashContrasena = servicios.GetRequiredService<IHashContrasena>();

        var usuario = new Usuario(
            Guid.NewGuid(),
            "Administrador",
            "NexusCommerce",
            correoNormalizado,
            RolUsuario.Administrador);

        repositorio.AgregarUsuario(usuario);
        repositorio.AgregarCredencial(new CredencialUsuario(
            Guid.NewGuid(),
            usuario.Id,
            hashContrasena.GenerarHash(usuario, contrasena)));

        await repositorio.GuardarCambiosAsync(cancellationToken);

        logger.LogInformation(
            "Administrador inicial creado: {Correo}",
            correoNormalizado);
    }
}
