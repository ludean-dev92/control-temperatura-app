using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Formatos",
                columns: table => new
                {
                    FormatoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinoId = table.Column<int>(type: "int", nullable: false),
                    FechaDescongelacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaProduccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RealizadoPor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisadoPor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formatos", x => x.FormatoId);
                });

            migrationBuilder.CreateTable(
                name: "Registros",
                columns: table => new
                {
                    RegistroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormatoId = table.Column<int>(type: "int", nullable: false),
                    NumeroCoche = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoProducto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraInicioDescongelado = table.Column<TimeSpan>(type: "time", nullable: true),
                    TemperaturaProducto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HoraInicioConsumo = table.Column<TimeSpan>(type: "time", nullable: true),
                    HoraFinConsumo = table.Column<TimeSpan>(type: "time", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registros", x => x.RegistroId);
                    table.ForeignKey(
                        name: "FK_Registros_Formatos_FormatoId",
                        column: x => x.FormatoId,
                        principalTable: "Formatos",
                        principalColumn: "FormatoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registros_FormatoId",
                table: "Registros",
                column: "FormatoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registros");

            migrationBuilder.DropTable(
                name: "Formatos");
        }
    }
}
