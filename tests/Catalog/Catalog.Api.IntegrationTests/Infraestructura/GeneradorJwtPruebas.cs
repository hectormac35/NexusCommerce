using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.Api.IntegrationTests.Infraestructura;

internal static class GeneradorJwtPruebas
{
    public static string GenerarAdministrador()
    {
        return Generar("Administrador");
    }

    public static string GenerarCliente()
    {
        return Generar("Cliente");
    }

    private static string Generar(
        string rol)
    {
        var ahora = DateTime.UtcNow;

        Claim[] claims =
        [
            new(
                JwtRegisteredClaimNames.Sub,
                Guid.NewGuid().ToString()),

            new(
                JwtRegisteredClaimNames.Email,
                "administrador@nexuscommerce.tests"),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()),

            new(
                ClaimTypes.NameIdentifier,
                Guid.NewGuid().ToString()),

            new(
                ClaimTypes.Name,
                "Administrador de pruebas"),

            new(
                ClaimTypes.Role,
                rol)
        ];

        var clave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                CatalogoApiFactory.JwtClave));

        var credenciales = new SigningCredentials(
            clave,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: CatalogoApiFactory.JwtEmisor,
            audience: CatalogoApiFactory.JwtAudiencia,
            claims: claims,
            notBefore: ahora,
            expires: ahora.AddMinutes(15),
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
