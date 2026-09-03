using Orders.Application.Abstractions.Persistence;
using Orders.Application.Common.Results;

namespace Orders.Application.Pedidos.Queries.GetOrderById;

public sealed class GetOrderByIdHandler
    : Abstractions.Messaging.IQueryHandler<
        GetOrderByIdQuery,
        GetOrderByIdResponse>
{
    private readonly IPedidoRepository _pedidoRepository;

    public GetOrderByIdHandler(
        IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<Result<GetOrderByIdResponse>> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObtenerPorIdAsync(
            request.PedidoId,
            cancellationToken);

        if (pedido is null)
        {
            return Result<GetOrderByIdResponse>.Fail(
                "Pedido no encontrado.");
        }

        var response = new GetOrderByIdResponse
        {
            PedidoId = pedido.Id,
            ClienteId = pedido.ClienteId,
            Estado = pedido.Estado.ToString(),
            Total = pedido.Total,
            FechaCreacionUtc = pedido.FechaCreacionUtc,
            Lineas = pedido.Lineas
                .Select(l => new GetOrderByIdResponse.GetOrderLineResponse
                {
                    ProductoId = l.ProductoId,
                    NombreProducto = l.NombreProducto,
                    PrecioUnitario = l.PrecioUnitario,
                    Cantidad = l.Cantidad
                })
                .ToList()
        };

        return Result<GetOrderByIdResponse>.Ok(response);
    }
}
