using Catalog.Domain.Productos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistencia.Configuraciones;

internal sealed class ProductoConfiguracion
    : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("productos");

        builder.HasKey(producto => producto.Id);

        builder.Property(producto => producto.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(producto => producto.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(producto => producto.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(1_000)
            .IsRequired();

        builder.Property(producto => producto.Precio)
            .HasColumnName("precio")
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(producto => producto.Stock)
            .HasColumnName("stock")
            .IsRequired();

        builder.Property(producto => producto.Categoria)
            .HasColumnName("categoria")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(producto => producto.EstaActivo)
            .HasColumnName("esta_activo")
            .IsRequired();

        builder.Property(producto => producto.FechaCreacionUtc)
            .HasColumnName("fecha_creacion_utc")
            .IsRequired();

        builder.Property(producto => producto.FechaActualizacionUtc)
            .HasColumnName("fecha_actualizacion_utc");

        builder.Ignore(producto => producto.TieneStock);

        builder.HasIndex(producto => producto.Nombre);

        builder.HasIndex(producto => producto.Categoria);

        builder.HasIndex(producto => producto.EstaActivo);
    }
}
