using Catalog.Domain.Productos;
using Catalog.Infrastructure.Persistencia.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistencia;

public sealed class CatalogoDbContext : DbContext
{
    public CatalogoDbContext(
        DbContextOptions<CatalogoDbContext> opciones)
        : base(opciones)
    {
    }

    public DbSet<Producto> Productos => Set<Producto>();

    public DbSet<MensajeOutbox> MensajesOutbox =>
        Set<MensajeOutbox>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CatalogoDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
