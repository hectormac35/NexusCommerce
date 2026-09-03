using Orders.Domain.Pedidos;

namespace Orders.Application.Abstractions.Persistence;

public interface IPedidoRepository
{
    Task AgregarAsync(
        Pedido pedido,
        CancellationToken cancellationToken = default);

    Task<Pedido?> ObtenerPorIdAsync(
        Guid pedidoId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Pedido>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);
}
