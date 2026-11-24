using Orders.Application.Abstractions.Catalogo;
using Orders.Application.Abstractions.Messaging;
using Orders.Application.Abstractions.Persistence;
using Orders.Application.Common.Results;
using Orders.Domain.Excepciones;
using Orders.Domain.Pedidos;

namespace Orders.Application.Pedidos.Commands.CreateOrder;

public sealed class CreateOrderHandler
    : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICatalogoProductos _catalogo;

    public CreateOrderHandler(
        IPedidoRepository pedidoRepository,
        IUnitOfWork unitOfWork,
        ICatalogoProductos catalogo)
    {
        _pedidoRepository = pedidoRepository;
        _unitOfWork = unitOfWork;
        _catalogo = catalogo;
    }

    public async Task<Result<CreateOrderResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        // Si el mismo producto viene en varias líneas, se suman las unidades.
        var lineasAgrupadas = request.Lineas
            .GroupBy(linea => linea.ProductoId)
            .Select(grupo => (ProductoId: grupo.Key, Cantidad: grupo.Sum(l => l.Cantidad)));

        try
        {
            Pedido pedido = Pedido.Crear(request.ClienteId);

            foreach (var (productoId, cantidad) in lineasAgrupadas)
            {
                ProductoCatalogo? producto = await _catalogo.ObtenerAsync(
                    productoId,
                    cancellationToken);

                if (producto is null)
                {
                    return Result<CreateOrderResponse>.Fail(
                        $"El producto {productoId} no existe.");
                }

                if (!producto.EstaActivo)
                {
                    return Result<CreateOrderResponse>.Fail(
                        $"El producto '{producto.Nombre}' no está disponible.");
                }

                if (producto.Stock < cantidad)
                {
                    return Result<CreateOrderResponse>.Fail(
                        $"No hay stock suficiente de '{producto.Nombre}' (quedan {producto.Stock}).");
                }

                // Nombre y precio vienen del catálogo: el cliente no puede fijarlos.
                pedido.AgregarLinea(
                    producto.Id,
                    producto.Nombre,
                    producto.Precio,
                    cantidad);
            }

            await _pedidoRepository.AgregarAsync(
                pedido,
                cancellationToken);

            await _unitOfWork.GuardarCambiosAsync(
                cancellationToken);

            return Result<CreateOrderResponse>.Ok(new CreateOrderResponse(
                pedido.Id,
                pedido.Total,
                pedido.FechaCreacionUtc));
        }
        catch (ExcepcionDominio excepcion)
        {
            return Result<CreateOrderResponse>.Fail(excepcion.Message);
        }
        catch (CatalogoNoDisponibleException)
        {
            return Result<CreateOrderResponse>.Fail(
                "El catálogo no está disponible en este momento. Inténtalo de nuevo.");
        }
    }
}
