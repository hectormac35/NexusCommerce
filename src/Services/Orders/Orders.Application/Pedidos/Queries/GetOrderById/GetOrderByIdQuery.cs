using Orders.Application.Abstractions.Messaging;

namespace Orders.Application.Pedidos.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid PedidoId)
    : IQuery<GetOrderByIdResponse>;
