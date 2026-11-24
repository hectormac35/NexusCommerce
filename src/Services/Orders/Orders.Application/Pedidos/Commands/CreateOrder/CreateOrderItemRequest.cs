namespace Orders.Application.Pedidos.Commands.CreateOrder;

/// <summary>
/// Lo único que decide el cliente es QUÉ producto y CUÁNTAS unidades.
/// El nombre y el precio se leen del catálogo en el servidor.
/// </summary>
public sealed record CreateOrderItemRequest(
    Guid ProductoId,
    int Cantidad);
