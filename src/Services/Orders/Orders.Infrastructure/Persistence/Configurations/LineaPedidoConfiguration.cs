using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Pedidos;

namespace Orders.Infrastructure.Persistence.Configurations;

internal sealed class LineaPedidoConfiguration
    : IEntityTypeConfiguration<LineaPedido>
{
    public void Configure(EntityTypeBuilder<LineaPedido> builder)
    {
        builder.ToTable("lineas_pedido");

        builder.HasKey(linea => linea.Id);

        builder.Property(linea => linea.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(linea => linea.PedidoId)
            .HasColumnName("pedido_id")
            .IsRequired();

        builder.Property(linea => linea.ProductoId)
            .HasColumnName("producto_id")
            .IsRequired();

        builder.Property(linea => linea.NombreProducto)
            .HasColumnName("nombre_producto")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(linea => linea.PrecioUnitario)
            .HasColumnName("precio_unitario")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(linea => linea.Cantidad)
            .HasColumnName("cantidad")
            .IsRequired();

        builder.Ignore(linea => linea.Subtotal);

        builder.HasIndex(linea => linea.PedidoId);

        builder.HasIndex(linea => linea.ProductoId);
    }
}
