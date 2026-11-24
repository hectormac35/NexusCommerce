using Orders.Application.Abstractions.Messaging;

namespace Orders.Application.Pedidos.Commands.CreateOrder;

/// <param name="ClienteId">Sale del token JWT del usuario, no del cuerpo de la petición.</param>
public sealed record CreateOrderCommand(
    Guid ClienteId,
    IReadOnlyCollection<CreateOrderItemRequest> Lineas)
    : ICommand<CreateOrderResponse>;
