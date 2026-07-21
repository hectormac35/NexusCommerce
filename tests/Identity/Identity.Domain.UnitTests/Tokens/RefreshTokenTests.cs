using Identity.Domain.Common;
using Identity.Domain.Tokens;

namespace Identity.Domain.UnitTests.Tokens;

public sealed class RefreshTokenTests
{
    [Fact]
    public void Constructor_ConDatosValidos_DebeCrearTokenValido()
    {
        var ahora = DateTime.UtcNow;

        var token = new RefreshToken(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "refresh-token-seguro",
            ahora,
            ahora.AddDays(7));

        Assert.False(token.EstaRevocado);
        Assert.False(token.HaExpirado);
        Assert.True(token.EsValido);
        Assert.Null(token.FechaRevocacionUtc);
        Assert.Null(token.ReemplazadoPorToken);
    }

    [Fact]
    public void Constructor_ConUsuarioIdVacio_DebeLanzarExcepcion()
    {
        var ahora = DateTime.UtcNow;

        Assert.Throws<ExcepcionDominio>(() =>
            new RefreshToken(
                Guid.NewGuid(),
                Guid.Empty,
                "refresh-token",
                ahora,
                ahora.AddDays(7)));
    }

    [Fact]
    public void Constructor_ConTokenVacio_DebeLanzarExcepcion()
    {
        var ahora = DateTime.UtcNow;

        Assert.Throws<ExcepcionDominio>(() =>
            new RefreshToken(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "",
                ahora,
                ahora.AddDays(7)));
    }

    [Fact]
    public void Constructor_ConExpiracionAnterior_DebeLanzarExcepcion()
    {
        var ahora = DateTime.UtcNow;

        Assert.Throws<ExcepcionDominio>(() =>
            new RefreshToken(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "refresh-token",
                ahora,
                ahora.AddMinutes(-1)));
    }

    [Fact]
    public void Revocar_DebeInvalidarToken()
    {
        var ahora = DateTime.UtcNow;
        var token = CrearToken(ahora);

        token.Revocar(ahora.AddMinutes(1));

        Assert.True(token.EstaRevocado);
        Assert.False(token.EsValido);
        Assert.NotNull(token.FechaRevocacionUtc);
    }

    [Fact]
    public void Revocar_ConReemplazo_DebeGuardarNuevoToken()
    {
        var ahora = DateTime.UtcNow;
        var token = CrearToken(ahora);

        token.Revocar(
            ahora.AddMinutes(1),
            "nuevo-refresh-token");

        Assert.Equal(
            "nuevo-refresh-token",
            token.ReemplazadoPorToken);
    }

    [Fact]
    public void Revocar_TokenYaRevocado_DebeLanzarExcepcion()
    {
        var ahora = DateTime.UtcNow;
        var token = CrearToken(ahora);

        token.Revocar(ahora.AddMinutes(1));

        Assert.Throws<ExcepcionDominio>(() =>
            token.Revocar(ahora.AddMinutes(2)));
    }

    [Fact]
    public void Revocar_ConMismoTokenComoReemplazo_DebeLanzarExcepcion()
    {
        var ahora = DateTime.UtcNow;
        var token = CrearToken(ahora);

        Assert.Throws<ExcepcionDominio>(() =>
            token.Revocar(
                ahora.AddMinutes(1),
                token.Token));
    }

    private static RefreshToken CrearToken(
        DateTime ahora)
    {
        return new RefreshToken(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "refresh-token-seguro",
            ahora,
            ahora.AddDays(7));
    }
}
