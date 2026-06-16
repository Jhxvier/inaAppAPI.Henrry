using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelacionCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "tb_Producto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tb_Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Categoria", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Producto_CategoriaId",
                table: "tb_Producto",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Producto_tb_Categoria_CategoriaId",
                table: "tb_Producto",
                column: "CategoriaId",
                principalTable: "tb_Categoria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Producto_tb_Categoria_CategoriaId",
                table: "tb_Producto");

            migrationBuilder.DropTable(
                name: "tb_Categoria");

            migrationBuilder.DropIndex(
                name: "IX_tb_Producto_CategoriaId",
                table: "tb_Producto");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "tb_Producto");
        }
    }
}
