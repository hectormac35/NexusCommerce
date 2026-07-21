using Identity.Domain.Tokens;
using Identity.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistencia.Configuraciones;

internal sealed class RefreshTokenConfiguracion
    : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(
        EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(token => token.Id);

        builder.Property(token => token.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(token => token.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.Property(token => token.Token)
            .HasColumnName("token")
            .HasMaxLength(512)
            .IsRequired();

        builder.HasIndex(token => token.Token)
            .IsUnique();

        builder.HasIndex(token => token.UsuarioId);

        builder.Property(token => token.FechaCreacionUtc)
            .HasColumnName("fecha_creacion_utc")
            .IsRequired();

        builder.Property(token => token.FechaExpiracionUtc)
            .HasColumnName("fecha_expiracion_utc")
            .IsRequired();

        builder.Property(token => token.FechaRevocacionUtc)
            .HasColumnName("fecha_revocacion_utc");

        builder.Property(token => token.ReemplazadoPorToken)
            .HasColumnName("reemplazado_por_token")
            .HasMaxLength(512);

        builder.Ignore(token => token.EstaRevocado);
        builder.Ignore(token => token.HaExpirado);
        builder.Ignore(token => token.EsValido);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(token => token.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
