using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistencia.Outbox;

internal sealed class MensajeOutboxConfiguracion
    : IEntityTypeConfiguration<MensajeOutbox>
{
    public void Configure(
        EntityTypeBuilder<MensajeOutbox> builder)
    {
        builder.ToTable("mensajes_outbox");

        builder.HasKey(mensaje => mensaje.Id);

        builder.Property(mensaje => mensaje.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(mensaje => mensaje.OcurridoEnUtc)
            .HasColumnName("ocurrido_en_utc")
            .IsRequired();

        builder.Property(mensaje => mensaje.Tipo)
            .HasColumnName("tipo")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(mensaje => mensaje.Contenido)
            .HasColumnName("contenido")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(mensaje => mensaje.ProcesadoEnUtc)
            .HasColumnName("procesado_en_utc");

        builder.Property(mensaje => mensaje.UltimoIntentoEnUtc)
            .HasColumnName("ultimo_intento_en_utc");

        builder.Property(mensaje => mensaje.Intentos)
            .HasColumnName("intentos")
            .HasDefaultValue(0);

        builder.Property(mensaje => mensaje.Error)
            .HasColumnName("error")
            .HasMaxLength(2000);

        builder.HasIndex(mensaje => new
        {
            mensaje.ProcesadoEnUtc,
            mensaje.OcurridoEnUtc
        })
            .HasDatabaseName(
                "ix_mensajes_outbox_pendientes");
    }
}
