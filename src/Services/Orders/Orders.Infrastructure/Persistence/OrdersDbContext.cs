using Microsoft.EntityFrameworkCore;
using Orders.Application.Abstractions.Persistence;
using Orders.Domain.Pedidos;

namespace Orders.Infrastructure.Persistence;

public sealed class OrdersDbContext : DbContext, IUnitOfWork
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pedido> Pedidos => Set<Pedido>();

    public DbSet<LineaPedido> LineasPedido => Set<LineaPedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(OrdersDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public Task<int> GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(cancellationToken);
    }
}
