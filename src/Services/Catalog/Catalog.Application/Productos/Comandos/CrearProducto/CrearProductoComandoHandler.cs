using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Application.Common.Results;
using Catalog.Domain.Productos;
using MediatR;

namespace Catalog.Application.Productos.Comandos.CrearProducto;

internal sealed class CrearProductoComandoHandler
    : IRequestHandler<CrearProductoComando, Resultado<Guid>>
{
    private readonly IRepositorioProductos _repositorio;

    public CrearProductoComandoHandler(
        IRepositorioProductos repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<Resultado<Guid>> Handle(
        CrearProductoComando request,
        CancellationToken cancellationToken)
    {
        var nombre = request.Nombre.Trim();

        var existeNombre = await _repositorio.ExisteNombreAsync(
            nombre,
            null,
            cancellationToken);

        if (existeNombre)
        {
            return ErroresProducto.NombreDuplicado;
        }

        var producto = new Producto(
            Guid.NewGuid(),
            nombre,
            request.Descripcion,
            request.Precio,
            request.Stock,
            request.Categoria);

        await _repositorio.AgregarAsync(
            producto,
            cancellationToken);

        await _repositorio.GuardarCambiosAsync(
            cancellationToken);

        return producto.Id;
    }
}
