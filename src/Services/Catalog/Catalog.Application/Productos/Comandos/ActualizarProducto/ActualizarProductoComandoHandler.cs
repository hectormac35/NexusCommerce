using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Application.Common.Results;
using MediatR;

namespace Catalog.Application.Productos.Comandos.ActualizarProducto;

internal sealed class ActualizarProductoComandoHandler
    : IRequestHandler<ActualizarProductoComando, Resultado>
{
    private readonly IRepositorioProductos _repositorio;

    public ActualizarProductoComandoHandler(
        IRepositorioProductos repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<Resultado> Handle(
        ActualizarProductoComando request,
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

        var nombre = request.Nombre.Trim();

        var existeNombre = await _repositorio.ExisteNombreAsync(
            nombre,
            request.Id,
            cancellationToken);

        if (existeNombre)
        {
            return Resultado.Fallo(
                ErroresProducto.NombreDuplicado);
        }

        producto.ActualizarInformacion(
            nombre,
            request.Descripcion,
            request.Precio,
            request.Categoria);

        await _repositorio.GuardarCambiosAsync(
            cancellationToken);

        return Resultado.Exito();
    }
}
