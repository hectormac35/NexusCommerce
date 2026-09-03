using Orders.Application.Abstractions.Persistence;
using Orders.Application.Common.Results;

namespace Orders.Application.Pedidos.Queries.GetOrders;

public sealed class GetOrdersHandler
    : Abstractions.Messaging.IQueryHandler<
        GetOrdersQuery,
        IReadOnlyList<GetOrdersResponse>>
{
    private readonly IPedidoRepository _pedidoRepository;

    public GetOrdersHandler(
        IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<Result<IReadOnlyList<GetOrdersResponse>>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var pedidos = await _pedidoRepository.ObtenerTodosAsync(
            cancellationToken);

        IReadOnlyList<GetOrdersResponse> response = pedidos
            .Select(pedido => new GetOrdersResponse
            {
                PedidoId = pedido.Id,
                ClienteId = pedido.ClienteId,
                Estado = pedido.Estado.ToString(),
                Total = pedido.Total,
                FechaCreacionUtc = pedido.FechaCreacionUtc
            })
            .ToList();

        return Result<IReadOnlyList<GetOrdersResponse>>.Ok(response);
    }
}
