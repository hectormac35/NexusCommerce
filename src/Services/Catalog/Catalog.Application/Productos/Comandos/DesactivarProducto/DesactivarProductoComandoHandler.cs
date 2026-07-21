using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Application.Common.Results;
using MediatR;

namespace Catalog.Application.Productos.Comandos.DesactivarProducto;

internal sealed class DesactivarProductoComandoHandler
    : IRequestHandler<DesactivarProductoComando, Resultado>
{
    private readonly IRepositorioProductos _repositorio;

    public DesactivarProductoComandoHandler(
        IRepositorioProductos repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<Resultado> Handle(
        DesactivarProductoComando request,
        CancellationToken cancellationToken)
    {
        var producto = await _repositorio.ObtenerPorIdAsync(
            request.Id,
            cancellationToken);

        if (producto is null)
        {
            return Resultado.Fallo(
                ErroresProducto.NoEncontrado(request.Id));
        }

        producto.Desactivar();

        await _repositorio.GuardarCambiosAsync(
            cancellationToken);

        return Resultado.Exito();
    }
}
