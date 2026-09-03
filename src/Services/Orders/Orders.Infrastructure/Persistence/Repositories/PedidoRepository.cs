using Microsoft.EntityFrameworkCore;
using Orders.Application.Abstractions.Persistence;
using Orders.Domain.Pedidos;

namespace Orders.Infrastructure.Persistence.Repositories;

internal sealed class PedidoRepository : IPedidoRepository
{
    private readonly OrdersDbContext _dbContext;

    public PedidoRepository(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AgregarAsync(
        Pedido pedido,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Pedidos.AddAsync(
            pedido,
            cancellationToken);
    }

    public Task<Pedido?> ObtenerPorIdAsync(
        Guid pedidoId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Pedidos
            .Include(pedido => pedido.Lineas)
            .SingleOrDefaultAsync(
                pedido => pedido.Id == pedidoId,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<Pedido>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Pedidos
            .AsNoTracking()
            .Include(pedido => pedido.Lineas)
            .OrderByDescending(pedido => pedido.FechaCreacionUtc)
            .ToListAsync(cancellationToken);
    }
}
