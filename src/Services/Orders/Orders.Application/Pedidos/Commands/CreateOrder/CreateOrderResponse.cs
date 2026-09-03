namespace Orders.Application.Pedidos.Commands.CreateOrder;

public sealed record CreateOrderResponse(
    Guid PedidoId,
    decimal Total,
    DateTime FechaCreacionUtc);
