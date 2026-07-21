using Identity.Domain.Common;
using Identity.Domain.Usuarios;

namespace Identity.Domain.UnitTests.Usuarios;

public sealed class CredencialUsuarioTests
{
    [Fact]
    public void Constructor_ConDatosValidos_DebeCrearCredencial()
    {
        var usuarioId = Guid.NewGuid();

        var credencial = new CredencialUsuario(
            Guid.NewGuid(),
            usuarioId,
            "hash-seguro-de-contrasena");

        Assert.NotEqual(Guid.Empty, credencial.Id);
        Assert.Equal(usuarioId, credencial.UsuarioId);
        Assert.Equal(
            "hash-seguro-de-contrasena",
            credencial.ContrasenaHash);

        Assert.Null(credencial.FechaActualizacionUtc);
    }

    [Fact]
    public void Constructor_ConUsuarioIdVacio_DebeLanzarExcepcion()
    {
        Assert.Throws<ExcepcionDominio>(() =>
            new CredencialUsuario(
                Guid.NewGuid(),
                Guid.Empty,
                "hash-seguro"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ConHashVacio_DebeLanzarExcepcion(
        string contrasenaHash)
    {
        Assert.Throws<ExcepcionDominio>(() =>
            new CredencialUsuario(
                Guid.NewGuid(),
                Guid.NewGuid(),
                contrasenaHash));
    }

    [Fact]
    public void ActualizarContrasenaHash_DebeModificarCredencial()
    {
        var credencial = CrearCredencial();

        credencial.ActualizarContrasenaHash(
            "nuevo-hash-seguro");

        Assert.Equal(
            "nuevo-hash-seguro",
            credencial.ContrasenaHash);

        Assert.NotNull(
            credencial.FechaActualizacionUtc);
    }

    [Fact]
    public void ActualizarContrasenaHash_ConMismoHash_DebeLanzarExcepcion()
    {
        var credencial = CrearCredencial();

        Assert.Throws<ExcepcionDominio>(() =>
            credencial.ActualizarContrasenaHash(
                credencial.ContrasenaHash));
    }

    private static CredencialUsuario CrearCredencial()
    {
        return new CredencialUsuario(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "hash-original-seguro");
    }
}
