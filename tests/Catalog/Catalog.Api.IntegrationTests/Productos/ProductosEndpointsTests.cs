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
        var nombre = CrearNombreUnico("Producto integración");

        var id = await CrearProductoAsync(nombre);

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

    [Fact]
    public async Task CrearProducto_ConDatosInvalidos_DebeDevolverBadRequest()
    {
        var peticion = new
        {
            nombre = "",
            descripcion = "",
            precio = -10m,
            stock = -1,
            categoria = ""
        };

        var respuesta = await _cliente.PostAsJsonAsync(
            "/api/catalogo/productos",
            peticion);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            respuesta.StatusCode);

        using var contenido = JsonDocument.Parse(
            await respuesta.Content.ReadAsStringAsync());

        Assert.Equal(
            400,
            contenido.RootElement
                .GetProperty("status")
                .GetInt32());

        Assert.True(
            contenido.RootElement.TryGetProperty(
                "errors",
                out var errores));

        Assert.Equal(
            JsonValueKind.Object,
            errores.ValueKind);
    }

    [Fact]
    public async Task CrearProducto_ConNombreDuplicado_DebeDevolverConflict()
    {
        var nombre = CrearNombreUnico("Producto duplicado");

        await CrearProductoAsync(nombre);

        var peticionDuplicada = CrearPeticion(nombre);

        var respuesta = await _cliente.PostAsJsonAsync(
            "/api/catalogo/productos",
            peticionDuplicada);

        Assert.Equal(
            HttpStatusCode.Conflict,
            respuesta.StatusCode);
    }

    [Fact]
    public async Task ActualizarProducto_ConProductoExistente_DebePersistirCambios()
    {
        var nombreInicial = CrearNombreUnico("Producto actualizable");
        var id = await CrearProductoAsync(nombreInicial);

        var nombreActualizado =
            CrearNombreUnico("Producto actualizado");

        var peticion = new
        {
            nombre = nombreActualizado,
            descripcion =
                "Descripción modificada mediante integración.",
            precio = 249.99m,
            categoria = "Actualizados"
        };

        var respuestaActualizacion =
            await _cliente.PutAsJsonAsync(
                $"/api/catalogo/productos/{id}",
                peticion);

        Assert.Equal(
            HttpStatusCode.NoContent,
            respuestaActualizacion.StatusCode);

        var respuestaConsulta = await _cliente.GetAsync(
            $"/api/catalogo/productos/{id}");

        Assert.Equal(
            HttpStatusCode.OK,
            respuestaConsulta.StatusCode);

        using var contenido = JsonDocument.Parse(
            await respuestaConsulta.Content.ReadAsStringAsync());

        Assert.Equal(
            nombreActualizado,
            contenido.RootElement
                .GetProperty("nombre")
                .GetString());

        Assert.Equal(
            249.99m,
            contenido.RootElement
                .GetProperty("precio")
                .GetDecimal());

        Assert.Equal(
            "Actualizados",
            contenido.RootElement
                .GetProperty("categoria")
                .GetString());
    }

    [Fact]
    public async Task ActualizarProducto_ConIdInexistente_DebeDevolverNotFound()
    {
        var peticion = new
        {
            nombre = CrearNombreUnico("Producto inexistente"),
            descripcion = "Producto que no existe.",
            precio = 99.99m,
            categoria = "Pruebas"
        };

        var respuesta = await _cliente.PutAsJsonAsync(
            $"/api/catalogo/productos/{Guid.NewGuid()}",
            peticion);

        Assert.Equal(
            HttpStatusCode.NotFound,
            respuesta.StatusCode);
    }

    [Fact]
    public async Task DesactivarProducto_ConProductoExistente_DebeOcultarlo()
    {
        var nombre = CrearNombreUnico("Producto desactivable");
        var id = await CrearProductoAsync(nombre);

        var respuestaDesactivacion =
            await _cliente.DeleteAsync(
                $"/api/catalogo/productos/{id}");

        Assert.Equal(
            HttpStatusCode.NoContent,
            respuestaDesactivacion.StatusCode);

        var respuestaConsulta = await _cliente.GetAsync(
            $"/api/catalogo/productos/{id}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            respuestaConsulta.StatusCode);
    }

    [Fact]
    public async Task DesactivarProducto_ConIdInexistente_DebeDevolverNotFound()
    {
        var respuesta = await _cliente.DeleteAsync(
            $"/api/catalogo/productos/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            respuesta.StatusCode);
    }

    private async Task<Guid> CrearProductoAsync(
        string nombre)
    {
        var respuesta = await _cliente.PostAsJsonAsync(
            "/api/catalogo/productos",
            CrearPeticion(nombre));

        Assert.Equal(
            HttpStatusCode.Created,
            respuesta.StatusCode);

        using var contenido = JsonDocument.Parse(
            await respuesta.Content.ReadAsStringAsync());

        return contenido.RootElement
            .GetProperty("id")
            .GetGuid();
    }

    private static object CrearPeticion(
        string nombre)
    {
        return new
        {
            nombre,
            descripcion =
                "Producto creado por una prueba de integración.",
            precio = 149.95m,
            stock = 12,
            categoria = "Integración"
        };
    }

    private static string CrearNombreUnico(
        string prefijo)
    {
        return $"{prefijo} {Guid.NewGuid():N}";
    }
}
