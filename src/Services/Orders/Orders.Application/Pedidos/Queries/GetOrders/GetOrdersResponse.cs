namespace Orders.Application.Pedidos.Queries.GetOrders;

public sealed class GetOrdersResponse
{
    public Guid PedidoId { get; init; }

    public Guid ClienteId { get; init; }

    public string Estado { get; init; } = string.Empty;

    public decimal Total { get; init; }

    public DateTime FechaCreacionUtc { get; init; }
}
