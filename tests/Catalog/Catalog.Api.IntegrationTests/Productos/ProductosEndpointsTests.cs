using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Catalog.Api.IntegrationTests.Infraestructura;

namespace Catalog.Api.IntegrationTests.Productos;

[Collection(ColeccionCatalogoApi.Nombre)]
public sealed class ProductosEndpointsTests
{
    private readonly HttpClient _cliente;

    public ProductosEndpointsTests(
        CatalogoApiFixture fixture)
    {
        _cliente = fixture.Cliente;
    }

    [Fact]
    public async Task HealthReady_ConPostgreSqlDisponible_DebeResponderOk()
    {
        var respuesta = await _cliente.GetAsync(
            "/health/ready");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
    }

    [Fact]
    public async Task ObtenerProductos_DebeDevolverProductosIniciales()
    {
        var respuesta = await _cliente.GetAsync(
            "/api/catalogo/productos");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        using var contenido = JsonDocument.Parse(
            await respuesta.Content.ReadAsStringAsync());

        Assert.Equal(
            JsonValueKind.Array,
            contenido.RootElement.ValueKind);

        Assert.NotEmpty(
            contenido.RootElement.EnumerateArray());
    }

    [Fact]
    public async Task CrearProducto_ConDatosValidos_DebePersistirlo()
    {
        var nombre =
            $"Producto integración {Guid.NewGuid():N}";

        var peticion = new
        {
            nombre,
            descripcion =
                "Producto creado por una prueba de integración.",
            precio = 149.95m,
            stock = 12,
            categoria = "Integración"
        };

        var respuestaCreacion = await _cliente.PostAsJsonAsync(
            "/api/catalogo/productos",
            peticion);

        Assert.Equal(
            HttpStatusCode.Created,
            respuestaCreacion.StatusCode);

        using var contenidoCreacion = JsonDocument.Parse(
            await respuestaCreacion.Content.ReadAsStringAsync());

        var id = contenidoCreacion.RootElement
            .GetProperty("id")
            .GetGuid();

        var respuestaConsulta = await _cliente.GetAsync(
            $"/api/catalogo/productos/{id}");

        Assert.Equal(
            HttpStatusCode.OK,
            respuestaConsulta.StatusCode);

        using var contenidoConsulta = JsonDocument.Parse(
            await respuestaConsulta.Content.ReadAsStringAsync());

        Assert.Equal(
            nombre,
            contenidoConsulta.RootElement
                .GetProperty("nombre")
                .GetString());
    }
}
