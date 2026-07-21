using Identity.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistencia.Configuraciones;

internal sealed class CredencialUsuarioConfiguracion
    : IEntityTypeConfiguration<CredencialUsuario>
{
    public void Configure(
        EntityTypeBuilder<CredencialUsuario> builder)
    {
        builder.ToTable("credenciales_usuarios");

        builder.HasKey(credencial => credencial.Id);

        builder.Property(credencial => credencial.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(credencial => credencial.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.HasIndex(credencial => credencial.UsuarioId)
            .IsUnique();

        builder.Property(
                credencial => credencial.ContrasenaHash)
            .HasColumnName("contrasena_hash")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(
                credencial => credencial.FechaCreacionUtc)
            .HasColumnName("fecha_creacion_utc")
            .IsRequired();

        builder.Property(
                credencial => credencial.FechaActualizacionUtc)
            .HasColumnName("fecha_actualizacion_utc");

        builder.HasOne<Usuario>()
            .WithOne()
            .HasForeignKey<CredencialUsuario>(
                credencial => credencial.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
