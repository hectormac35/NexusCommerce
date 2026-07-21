using Catalog.Domain.Productos;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CatalogoDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
