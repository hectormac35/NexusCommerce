using Identity.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistencia.Configuraciones;

internal sealed class UsuarioConfiguracion
    : IEntityTypeConfiguration<Usuario>
{
    public void Configure(
        EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(usuario => usuario.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(usuario => usuario.Apellidos)
            .HasColumnName("apellidos")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(usuario => usuario.Correo)
            .HasColumnName("correo")
            .HasMaxLength(320)
            .IsRequired();

        builder.HasIndex(usuario => usuario.Correo)
            .IsUnique();

        builder.Property(usuario => usuario.Rol)
            .HasColumnName("rol")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(usuario => usuario.Estado)
            .HasColumnName("estado")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(usuario => usuario.FechaCreacionUtc)
            .HasColumnName("fecha_creacion_utc")
            .IsRequired();

        builder.Property(usuario => usuario.FechaActualizacionUtc)
            .HasColumnName("fecha_actualizacion_utc");

        builder.Ignore(usuario => usuario.NombreCompleto);
        builder.Ignore(usuario => usuario.EstaActivo);
    }
}
