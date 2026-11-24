using Orders.Application.Abstractions.Messaging;

namespace Orders.Application.Pedidos.Queries.GetOrders;

/// <param name="ClienteId">
/// Si tiene valor, solo se devuelven los pedidos de ese cliente.
/// <c>null</c> = todos los pedidos (solo para administradores).
/// </param>
public sealed record GetOrdersQuery(Guid? ClienteId = null)
    : IQuery<IReadOnlyList<GetOrdersResponse>>;
