using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Pedidos;

namespace Orders.Infrastructure.Persistence.Configurations;

internal sealed class PedidoConfiguration
    : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("pedidos");

        builder.HasKey(pedido => pedido.Id);

        builder.Property(pedido => pedido.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(pedido => pedido.ClienteId)
            .HasColumnName("cliente_id")
            .IsRequired();

        builder.Property(pedido => pedido.Estado)
            .HasColumnName("estado")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(pedido => pedido.FechaCreacionUtc)
            .HasColumnName("fecha_creacion_utc")
            .IsRequired();

        builder.Property(pedido => pedido.FechaActualizacionUtc)
            .HasColumnName("fecha_actualizacion_utc");

        builder.Property(pedido => pedido.FechaCancelacionUtc)
            .HasColumnName("fecha_cancelacion_utc");

        builder.Ignore(pedido => pedido.Total);

        builder.HasMany(pedido => pedido.Lineas)
            .WithOne()
            .HasForeignKey(linea => linea.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(pedido => pedido.Lineas)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(pedido => pedido.ClienteId);

        builder.HasIndex(pedido => pedido.Estado);

        builder.HasIndex(pedido => pedido.FechaCreacionUtc);
    }
}
