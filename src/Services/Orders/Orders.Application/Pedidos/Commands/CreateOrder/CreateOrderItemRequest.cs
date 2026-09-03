namespace Orders.Application.Pedidos.Commands.CreateOrder;

public sealed record CreateOrderItemRequest(
    Guid ProductoId,
    string NombreProducto,
    decimal PrecioUnitario,
    int Cantidad);
