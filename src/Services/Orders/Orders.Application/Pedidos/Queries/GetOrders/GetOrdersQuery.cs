using Orders.Application.Abstractions.Messaging;

namespace Orders.Application.Pedidos.Queries.GetOrders;

public sealed record GetOrdersQuery()
    : IQuery<IReadOnlyList<GetOrdersResponse>>;
