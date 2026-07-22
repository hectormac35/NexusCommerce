using Catalog.Application.Abstracciones.Mensajeria;
using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Application.Common.Results;
using Catalog.Application.Eventos.Integracion;
using Catalog.Domain.Productos;
using MediatR;

namespace Catalog.Application.Productos.Comandos.CrearProducto;

internal sealed class CrearProductoComandoHandler
    : IRequestHandler<CrearProductoComando, Resultado<Guid>>
{
    private readonly IRepositorioProductos _repositorio;
    private readonly IOutbox _outbox;

    public CrearProductoComandoHandler(
        IRepositorioProductos repositorio,
        IOutbox outbox)
    {
        _repositorio = repositorio;
        _outbox = outbox;
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

        var evento = new ProductoCreadoEvento(
            Guid.NewGuid(),
            DateTime.UtcNow,
            producto.Id,
            producto.Nombre,
            producto.Precio,
            producto.Stock,
            producto.Categoria);

        await _repositorio.AgregarAsync(
            producto,
            cancellationToken);

        await _outbox.AgregarAsync(
            evento,
            cancellationToken);

        await _repositorio.GuardarCambiosAsync(
            cancellationToken);

        return producto.Id;
    }
}
