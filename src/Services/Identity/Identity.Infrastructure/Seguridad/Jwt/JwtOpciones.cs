namespace Identity.Infrastructure.Seguridad.Jwt;

internal sealed class JwtOpciones
{
    public const string Seccion = "Jwt";

    public string Emisor { get; init; } = string.Empty;

    public string Audiencia { get; init; } = string.Empty;

    public string Clave { get; init; } = string.Empty;

    public int DuracionAccessTokenMinutos { get; init; }

    public int DuracionRefreshTokenDias { get; init; }
}
