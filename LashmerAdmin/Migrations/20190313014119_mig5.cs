using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LashmerAdmin.Migrations
{
    public partial class mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Product_ProductId",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "ProductDetails",
                table: "Product",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItem",
                newName: "ProductOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItem",
                newName: "IX_OrderItem_ProductOptionId");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Product",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductOptions",
                columns: table => new
                {
                    ProductOptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    OptionName = table.Column<string>(nullable: true),
                    OptionValue = table.Column<string>(nullable: true),
                    OptionDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptions", x => x.ProductOptionId);
                    table.ForeignKey(
                        name: "FK_ProductOptions_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptions_ProductId",
                table: "ProductOptions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ProductOptions_ProductOptionId",
                table: "OrderItem",
                column: "ProductOptionId",
                principalTable: "ProductOptions",
                principalColumn: "ProductOptionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ProductOptions_ProductOptionId",
                table: "OrderItem");

            migrationBuilder.DropTable(
                name: "ProductOptions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Product",
                newName: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "ProductOptionId",
                table: "OrderItem",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_ProductOptionId",
                table: "OrderItem",
                newName: "IX_OrderItem_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Product",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Product_ProductId",
                table: "OrderItem",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
