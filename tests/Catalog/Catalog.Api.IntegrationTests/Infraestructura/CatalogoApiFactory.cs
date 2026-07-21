using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Catalog.Api.IntegrationTests.Infraestructura;

internal sealed class CatalogoApiFactory(
    string cadenaConexion)
    : WebApplicationFactory<Program>
{
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
                            cadenaConexion
                    });
            });
    }
}
