using Identity.Domain.Tokens;
using Identity.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistencia;

public sealed class IdentidadDbContext(
    DbContextOptions<IdentidadDbContext> options)
    : DbContext(options)
{
    public DbSet<Usuario> Usuarios =>
        Set<Usuario>();

    public DbSet<RefreshToken> RefreshTokens =>
        Set<RefreshToken>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IdentidadDbContext).Assembly);
    }
}
