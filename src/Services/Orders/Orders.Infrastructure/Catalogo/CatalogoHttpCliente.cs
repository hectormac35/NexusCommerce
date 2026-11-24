using System.Net;
using System.Net.Http.Json;
using Orders.Application.Abstractions.Catalogo;

namespace Orders.Infrastructure.Catalogo;

/// <summary>
/// Cliente HTTP del microservicio de catálogo (GET /api/catalogo/productos/{id}).
/// </summary>
internal sealed class CatalogoHttpCliente : ICatalogoProductos
{
    private readonly HttpClient _httpClient;

    public CatalogoHttpCliente(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductoCatalogo?> ObtenerAsync(
        Guid productoId,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage respuesta;

        try
        {
            respuesta = await _httpClient.GetAsync(
                $"api/catalogo/productos/{productoId}",
                cancellationToken);
        }
        catch (HttpRequestException excepcion)
        {
            throw new CatalogoNoDisponibleException(
                "No se pudo contactar con el catálogo.",
                excepcion);
        }
        catch (TaskCanceledException excepcion) when (!cancellationToken.IsCancellationRequested)
        {
            throw new CatalogoNoDisponibleException(
                "El catálogo no respondió a tiempo.",
                excepcion);
        }

        using (respuesta)
        {
            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new CatalogoNoDisponibleException(
                    $"El catálogo respondió {(int)respuesta.StatusCode}.");
            }

            return await respuesta.Content.ReadFromJsonAsync<ProductoCatalogo>(
                cancellationToken: cancellationToken);
        }
    }
}
