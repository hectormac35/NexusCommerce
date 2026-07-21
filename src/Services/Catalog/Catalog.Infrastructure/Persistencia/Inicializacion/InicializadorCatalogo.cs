using Catalog.Domain.Productos;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistencia.Inicializacion;

public static class InicializadorCatalogo
{
    public static async Task InicializarAsync(
        CatalogoDbContext contexto,
        CancellationToken cancellationToken = default)
    {
        await contexto.Database.MigrateAsync(cancellationToken);

        if (await contexto.Productos.AnyAsync(cancellationToken))
        {
            return;
        }

        Producto[] productos =
        [
            new(
                Guid.Parse("2f8ec43e-5a75-4de4-a8e4-d49c2c270101"),
                "Teclado mecánico",
                "Teclado mecánico compacto con iluminación RGB.",
                89.99m,
                25,
                "Periféricos"),

            new(
                Guid.Parse("2f8ec43e-5a75-4de4-a8e4-d49c2c270102"),
                "Ratón inalámbrico",
                "Ratón ergonómico con conexión inalámbrica.",
                39.95m,
                40,
                "Periféricos"),

            new(
                Guid.Parse("2f8ec43e-5a75-4de4-a8e4-d49c2c270103"),
                "Monitor 27 pulgadas",
                "Monitor IPS con resolución 1440p.",
                299.90m,
                12,
                "Monitores")
        ];

        await contexto.Productos.AddRangeAsync(
            productos,
            cancellationToken);

        await contexto.SaveChangesAsync(cancellationToken);
    }
}
