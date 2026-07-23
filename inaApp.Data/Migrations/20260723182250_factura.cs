using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class factura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_Factura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroFactura = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Factura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_Factura_tb_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "tb_Cliente",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_FacturaDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_FacturaDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_FacturaDetalle_tb_Factura_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "tb_Factura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_FacturaDetalle_tb_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "tb_Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Factura_ClienteId",
                table: "tb_Factura",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Factura_NumeroFactura",
                table: "tb_Factura",
                column: "NumeroFactura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_FacturaDetalle_FacturaId",
                table: "tb_FacturaDetalle",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_FacturaDetalle_ProductoId",
                table: "tb_FacturaDetalle",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_FacturaDetalle");

            migrationBuilder.DropTable(
                name: "tb_Factura");
        }
    }
}
