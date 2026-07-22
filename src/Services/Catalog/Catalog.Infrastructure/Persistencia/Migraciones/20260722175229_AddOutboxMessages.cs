using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AddOutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mensajes_outbox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ocurrido_en_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tipo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    contenido = table.Column<string>(type: "jsonb", nullable: false),
                    procesado_en_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ultimo_intento_en_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    intentos = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    error = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mensajes_outbox", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_mensajes_outbox_pendientes",
                table: "mensajes_outbox",
                columns: new[] { "procesado_en_utc", "ocurrido_en_utc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mensajes_outbox");
        }
    }
}
