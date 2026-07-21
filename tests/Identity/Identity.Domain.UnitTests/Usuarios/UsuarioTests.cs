using Identity.Domain.Common;
using Identity.Domain.Usuarios;

namespace Identity.Domain.UnitTests.Usuarios;

public sealed class UsuarioTests
{
    [Fact]
    public void Constructor_ConDatosValidos_DebeCrearUsuarioActivo()
    {
        var usuario = CrearUsuario();

        Assert.NotEqual(Guid.Empty, usuario.Id);
        Assert.Equal("Héctor", usuario.Nombre);
        Assert.Equal("Macarrilla", usuario.Apellidos);
        Assert.Equal("hector@example.com", usuario.Correo);
        Assert.Equal("Héctor Macarrilla", usuario.NombreCompleto);
        Assert.Equal(RolUsuario.Cliente, usuario.Rol);
        Assert.Equal(EstadoUsuario.Activo, usuario.Estado);
        Assert.True(usuario.EstaActivo);
    }

    [Fact]
    public void Constructor_DebeNormalizarCorreo()
    {
        var usuario = new Usuario(
            Guid.NewGuid(),
            "Héctor",
            "Macarrilla",
            "  HECTOR@EXAMPLE.COM  ");

        Assert.Equal(
            "hector@example.com",
            usuario.Correo);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("correo-invalido")]
    public void Constructor_ConCorreoInvalido_DebeLanzarExcepcion(
        string correo)
    {
        Assert.Throws<ExcepcionDominio>(() =>
            new Usuario(
                Guid.NewGuid(),
                "Héctor",
                "Macarrilla",
                correo));
    }

    [Fact]
    public void Constructor_ConIdVacio_DebeLanzarExcepcion()
    {
        Assert.Throws<ExcepcionDominio>(() =>
            new Usuario(
                Guid.Empty,
                "Héctor",
                "Macarrilla",
                "hector@example.com"));
    }

    [Fact]
    public void ActualizarPerfil_ConDatosValidos_DebeModificarNombre()
    {
        var usuario = CrearUsuario();

        usuario.ActualizarPerfil(
            "Héctor Manuel",
            "Macarrilla Maluenda");

        Assert.Equal(
            "Héctor Manuel",
            usuario.Nombre);

        Assert.Equal(
            "Macarrilla Maluenda",
            usuario.Apellidos);

        Assert.NotNull(usuario.FechaActualizacionUtc);
    }

    [Fact]
    public void CambiarCorreo_DebeNormalizarNuevoCorreo()
    {
        var usuario = CrearUsuario();

        usuario.CambiarCorreo(
            "  NUEVO@EXAMPLE.COM ");

        Assert.Equal(
            "nuevo@example.com",
            usuario.Correo);
    }

    [Fact]
    public void CambiarRol_ConRolValido_DebeActualizarRol()
    {
        var usuario = CrearUsuario();

        usuario.CambiarRol(
            RolUsuario.Administrador);

        Assert.Equal(
            RolUsuario.Administrador,
            usuario.Rol);

        Assert.NotNull(usuario.FechaActualizacionUtc);
    }

    [Fact]
    public void Bloquear_UsuarioActivo_DebeCambiarEstado()
    {
        var usuario = CrearUsuario();

        usuario.Bloquear();

        Assert.Equal(
            EstadoUsuario.Bloqueado,
            usuario.Estado);

        Assert.False(usuario.EstaActivo);
    }

    [Fact]
    public void Desbloquear_UsuarioBloqueado_DebeActivarlo()
    {
        var usuario = CrearUsuario();

        usuario.Bloquear();
        usuario.Desbloquear();

        Assert.Equal(
            EstadoUsuario.Activo,
            usuario.Estado);

        Assert.True(usuario.EstaActivo);
    }

    [Fact]
    public void Desbloquear_UsuarioActivo_DebeLanzarExcepcion()
    {
        var usuario = CrearUsuario();

        Assert.Throws<ExcepcionDominio>(
            usuario.Desbloquear);
    }

    [Fact]
    public void Desactivar_DebeCambiarEstado()
    {
        var usuario = CrearUsuario();

        usuario.Desactivar();

        Assert.Equal(
            EstadoUsuario.Desactivado,
            usuario.Estado);

        Assert.False(usuario.EstaActivo);
    }

    [Fact]
    public void Bloquear_UsuarioDesactivado_DebeLanzarExcepcion()
    {
        var usuario = CrearUsuario();

        usuario.Desactivar();

        Assert.Throws<ExcepcionDominio>(
            usuario.Bloquear);
    }

    private static Usuario CrearUsuario()
    {
        return new Usuario(
            Guid.NewGuid(),
            "Héctor",
            "Macarrilla",
            "hector@example.com");
    }
}
