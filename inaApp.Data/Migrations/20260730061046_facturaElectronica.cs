using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class facturaElectronica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "tb_Producto",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DescuentoMaximo",
                table: "tb_Producto",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte>(
                name: "ImpuestoAplicable",
                table: "tb_Producto",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<decimal>(
                name: "PorcentajeImpuesto",
                table: "tb_Producto",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 13m);

            migrationBuilder.Sql(
                """
                UPDATE tb_Producto
                SET Codigo = CONCAT('PROD-', Id)
                WHERE Codigo IS NULL OR LTRIM(RTRIM(Codigo)) = '';
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "tb_Producto",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "tb_FacturaDetalle",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PorcentajeDescuento",
                table: "tb_FacturaDetalle",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PorcentajeImpuesto",
                table: "tb_FacturaDetalle",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(
                """
                UPDATE detalle
                SET detalle.PorcentajeImpuesto = producto.PorcentajeImpuesto
                FROM tb_FacturaDetalle AS detalle
                INNER JOIN tb_Producto AS producto
                    ON producto.Id = detalle.ProductoId;
                """);

            migrationBuilder.Sql(
                """
                UPDATE tb_FacturaDetalle
                SET
                    Descuento = 0,
                    PorcentajeDescuento = 0,
                    Impuesto =
                        ROUND(
                            (Subtotal - 0)
                            * PorcentajeImpuesto
                            / 100.0,
                            2
                        ),
                    TotalLinea =
                        ROUND(
                            (Subtotal - 0)
                            +
                            (
                                (Subtotal - 0)
                                * PorcentajeImpuesto
                                / 100.0
                            ),
                            2
                        );
                """);

            migrationBuilder.AddColumn<int>(
                name: "FacturaOrigenId",
                table: "tb_Factura",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo",
                table: "tb_Factura",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroDocumentoOriginal",
                table: "tb_Factura",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "TipoDocumento",
                table: "tb_Factura",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "TipoDocumentoOriginal",
                table: "tb_Factura",
                type: "tinyint",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE factura
                SET
                    factura.Subtotal = totales.Subtotal,
                    factura.Impuesto = totales.Impuesto,
                    factura.Descuento = totales.Descuento,
                    factura.Total =
                        totales.Subtotal
                        + totales.Impuesto
                        - totales.Descuento
                FROM tb_Factura AS factura
                INNER JOIN
                (
                    SELECT
                        FacturaId,
                        SUM(Subtotal) AS Subtotal,
                        SUM(Impuesto) AS Impuesto,
                        SUM(Descuento) AS Descuento
                    FROM tb_FacturaDetalle
                    GROUP BY FacturaId
                ) AS totales
                    ON totales.FacturaId = factura.Id;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_tb_Producto_Codigo",
                table: "tb_Producto",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_Factura_FacturaOrigenId",
                table: "tb_Factura",
                column: "FacturaOrigenId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Factura_tb_Factura_FacturaOrigenId",
                table: "tb_Factura",
                column: "FacturaOrigenId",
                principalTable: "tb_Factura",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_tb_Factura_tb_Factura_FacturaOrigenId",
                table: "tb_Factura");

            migrationBuilder.DropIndex(
                name: "IX_tb_Producto_Codigo",
                table: "tb_Producto");

            migrationBuilder.DropIndex(
                name: "IX_tb_Factura_FacturaOrigenId",
                table: "tb_Factura");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "tb_Producto");

            migrationBuilder.DropColumn(
                name: "DescuentoMaximo",
                table: "tb_Producto");

            migrationBuilder.DropColumn(
                name: "ImpuestoAplicable",
                table: "tb_Producto");

            migrationBuilder.DropColumn(
                name: "PorcentajeImpuesto",
                table: "tb_Producto");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "tb_FacturaDetalle");

            migrationBuilder.DropColumn(
                name: "PorcentajeDescuento",
                table: "tb_FacturaDetalle");

            migrationBuilder.DropColumn(
                name: "PorcentajeImpuesto",
                table: "tb_FacturaDetalle");

            migrationBuilder.DropColumn(
                name: "FacturaOrigenId",
                table: "tb_Factura");

            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "tb_Factura");

            migrationBuilder.DropColumn(
                name: "NumeroDocumentoOriginal",
                table: "tb_Factura");

            migrationBuilder.DropColumn(
                name: "TipoDocumento",
                table: "tb_Factura");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoOriginal",
                table: "tb_Factura");
        }
    }
}