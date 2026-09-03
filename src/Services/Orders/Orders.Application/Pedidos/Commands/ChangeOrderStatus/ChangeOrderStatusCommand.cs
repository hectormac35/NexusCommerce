using Orders.Application.Abstractions.Messaging;

namespace Orders.Application.Pedidos.Commands.ChangeOrderStatus;

public sealed record ChangeOrderStatusCommand(
    Guid PedidoId,
    string NuevoEstado)
    : ICommand;
