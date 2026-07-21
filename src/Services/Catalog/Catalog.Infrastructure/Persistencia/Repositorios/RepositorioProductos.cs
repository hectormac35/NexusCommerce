using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Domain.Productos;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistencia.Repositorios;

internal sealed class RepositorioProductos : IRepositorioProductos
{
    private readonly CatalogoDbContext _contexto;

    public RepositorioProductos(CatalogoDbContext contexto)
    {
        _contexto = contexto;
    }

    public Task<bool> ExisteNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        return _contexto.Productos.AnyAsync(
            producto => EF.Functions.ILike(producto.Nombre, nombre),
            cancellationToken);
    }

    public async Task AgregarAsync(
        Producto producto,
        CancellationToken cancellationToken = default)
    {
        await _contexto.Productos.AddAsync(
            producto,
            cancellationToken);
    }

    public Task<int> GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        return _contexto.SaveChangesAsync(cancellationToken);
    }
}
