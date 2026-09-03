using Orders.Application.Abstractions.Messaging;

namespace Orders.Application.Pedidos.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid ClienteId,
    IReadOnlyCollection<CreateOrderItemRequest> Lineas)
    : ICommand<CreateOrderResponse>;
