using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Catalog.Api.IntegrationTests.Infraestructura;

internal sealed class CatalogoApiFactory(
    string cadenaConexion)
    : WebApplicationFactory<Program>
{
    // Debe coincidir con Jwt:Clave de Catalog.Api/appsettings.json: Program.cs lee la
    // clave al construir la aplicación, antes de que se apliquen estos valores en memoria.
    public const string JwtClave =
        "NexusCommerce-Development-Key-2026-Muy-Segura-Para-JWT";

    public const string JwtEmisor =
        "NexusCommerce.Identity";

    public const string JwtAudiencia =
        "NexusCommerce.Clientes";

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");

        builder.ConfigureAppConfiguration(
            (_, configuracion) =>
            {
                configuracion.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:Catalogo"] =
                            cadenaConexion,

                        ["Jwt:Clave"] =
                            JwtClave,

                        ["Jwt:Emisor"] =
                            JwtEmisor,

                        ["Jwt:Audiencia"] =
                            JwtAudiencia
                    });
            });
    }
}
