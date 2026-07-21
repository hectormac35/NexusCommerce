using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Domain.Productos;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistencia.Consultas;

internal sealed class ConsultaProductos : IConsultaProductos
{
    private readonly CatalogoDbContext _contexto;

    public ConsultaProductos(CatalogoDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyCollection<Producto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        return await _contexto.Productos
            .AsNoTracking()
            .OrderBy(producto => producto.Nombre)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Producto?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _contexto.Productos
            .AsNoTracking()
            .FirstOrDefaultAsync(
                producto => producto.Id == id,
                cancellationToken);
    }
}
