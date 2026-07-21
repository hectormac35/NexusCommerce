using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Application.Abstracciones.Seguridad;
using Identity.Domain.Usuarios;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Seguridad.Jwt;

internal sealed class GeneradorTokens(
    IOptions<JwtOpciones> opciones)
    : IGeneradorTokens
{
    private readonly JwtOpciones _opciones = opciones.Value;

    public TokenGenerado Generar(Usuario usuario)
    {
        var ahora = DateTime.UtcNow;

        var expiracionAccessToken = ahora.AddMinutes(
            _opciones.DuracionAccessTokenMinutos);

        var clave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_opciones.Clave));

        var credenciales = new SigningCredentials(
            clave,
            SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new(
                JwtRegisteredClaimNames.Sub,
                usuario.Id.ToString()),

            new(
                JwtRegisteredClaimNames.Email,
                usuario.Correo),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()),

            new(
                ClaimTypes.NameIdentifier,
                usuario.Id.ToString()),

            new(
                ClaimTypes.Name,
                usuario.NombreCompleto),

            new(
                ClaimTypes.Role,
                usuario.Rol.ToString())
        ];

        var jwt = new JwtSecurityToken(
            issuer: _opciones.Emisor,
            audience: _opciones.Audiencia,
            claims: claims,
            notBefore: ahora,
            expires: expiracionAccessToken,
            signingCredentials: credenciales);

        var accessToken =
            new JwtSecurityTokenHandler().WriteToken(jwt);

        var refreshTokenBytes =
            RandomNumberGenerator.GetBytes(64);

        var refreshToken =
            Convert.ToBase64String(refreshTokenBytes);

        var refreshTokenHash =
            Convert.ToHexString(
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(refreshToken)));

        var expiracionRefreshToken = ahora.AddDays(
            _opciones.DuracionRefreshTokenDias);

        return new TokenGenerado(
            accessToken,
            refreshToken,
            refreshTokenHash,
            _opciones.DuracionAccessTokenMinutos * 60,
            expiracionRefreshToken);
    }
}
