using Orders.Application.Abstractions.Messaging;
using Orders.Application.Abstractions.Persistence;
using Orders.Application.Common.Results;
using Orders.Domain.Pedidos;

namespace Orders.Application.Pedidos.Commands.CreateOrder;

public sealed class CreateOrderHandler
    : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderHandler(
        IPedidoRepository pedidoRepository,
        IUnitOfWork unitOfWork)
    {
        _pedidoRepository = pedidoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateOrderResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        Pedido pedido = Pedido.Crear(request.ClienteId);

        foreach (CreateOrderItemRequest linea in request.Lineas)
        {
            pedido.AgregarLinea(
                linea.ProductoId,
                linea.NombreProducto,
                linea.PrecioUnitario,
                linea.Cantidad);
        }

        await _pedidoRepository.AgregarAsync(
            pedido,
            cancellationToken);

        await _unitOfWork.GuardarCambiosAsync(
            cancellationToken);

        var response = new CreateOrderResponse(
            pedido.Id,
            pedido.Total,
            pedido.FechaCreacionUtc);

        return Result<CreateOrderResponse>.Ok(response);
    }
}
