using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Catalog.Api.IntegrationTests.Infraestructura;

internal sealed class CatalogoApiFactory(
    string cadenaConexion)
    : WebApplicationFactory<Program>
{
    public const string JwtClave =
        "NexusCommerce-Development-Key-2026-Super-Segura-Minimo-32";

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
