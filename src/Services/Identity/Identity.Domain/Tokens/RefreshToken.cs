using Identity.Domain.Common;

namespace Identity.Domain.Tokens;

public sealed class RefreshToken
{
    private RefreshToken()
    {
    }

    public RefreshToken(
        Guid id,
        Guid usuarioId,
        string token,
        DateTime fechaCreacionUtc,
        DateTime fechaExpiracionUtc)
    {
        if (id == Guid.Empty)
        {
            throw new ExcepcionDominio(
                "El identificador del refresh token es obligatorio.");
        }

        if (usuarioId == Guid.Empty)
        {
            throw new ExcepcionDominio(
                "El identificador del usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ExcepcionDominio(
                "El valor del refresh token es obligatorio.");
        }

        if (fechaExpiracionUtc <= fechaCreacionUtc)
        {
            throw new ExcepcionDominio(
                "La fecha de expiración debe ser posterior a la fecha de creación.");
        }

        Id = id;
        UsuarioId = usuarioId;
        Token = token.Trim();
        FechaCreacionUtc = fechaCreacionUtc;
        FechaExpiracionUtc = fechaExpiracionUtc;
    }

    public Guid Id { get; private set; }

    public Guid UsuarioId { get; private set; }

    public string Token { get; private set; } = string.Empty;

    public DateTime FechaCreacionUtc { get; private set; }

    public DateTime FechaExpiracionUtc { get; private set; }

    public DateTime? FechaRevocacionUtc { get; private set; }

    public string? ReemplazadoPorToken { get; private set; }

    public bool EstaRevocado =>
        FechaRevocacionUtc.HasValue;

    public bool HaExpirado =>
        DateTime.UtcNow >= FechaExpiracionUtc;

    public bool EsValido =>
        !EstaRevocado && !HaExpirado;

    public void Revocar(
        DateTime fechaRevocacionUtc,
        string? reemplazadoPorToken = null)
    {
        if (EstaRevocado)
        {
            throw new ExcepcionDominio(
                "El refresh token ya se encuentra revocado.");
        }

        if (fechaRevocacionUtc < FechaCreacionUtc)
        {
            throw new ExcepcionDominio(
                "La fecha de revocación no puede ser anterior a la creación.");
        }

        if (!string.IsNullOrWhiteSpace(reemplazadoPorToken) &&
            reemplazadoPorToken.Trim() == Token)
        {
            throw new ExcepcionDominio(
                "El token de reemplazo no puede ser igual al token actual.");
        }

        FechaRevocacionUtc = fechaRevocacionUtc;
        ReemplazadoPorToken =
            string.IsNullOrWhiteSpace(reemplazadoPorToken)
                ? null
                : reemplazadoPorToken.Trim();
    }
}
