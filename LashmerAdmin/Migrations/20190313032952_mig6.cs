using Microsoft.EntityFrameworkCore.Migrations;

namespace LashmerAdmin.Migrations
{
    public partial class mig6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ProductOptions_ProductOptionId",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptions_Product_ProductId",
                table: "ProductOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductOptions",
                table: "ProductOptions");

            migrationBuilder.RenameTable(
                name: "ProductOptions",
                newName: "ProductOption");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOptions_ProductId",
                table: "ProductOption",
                newName: "IX_ProductOption_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOption",
                table: "ProductOption",
                column: "ProductOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ProductOption_ProductOptionId",
                table: "OrderItem",
                column: "ProductOptionId",
                principalTable: "ProductOption",
                principalColumn: "ProductOptionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOption_Product_ProductId",
                table: "ProductOption",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ProductOption_ProductOptionId",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOption_Product_ProductId",
                table: "ProductOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductOption",
                table: "ProductOption");

            migrationBuilder.RenameTable(
                name: "ProductOption",
                newName: "ProductOptions");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOption_ProductId",
                table: "ProductOptions",
                newName: "IX_ProductOptions_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOptions",
                table: "ProductOptions",
                column: "ProductOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ProductOptions_ProductOptionId",
                table: "OrderItem",
                column: "ProductOptionId",
                principalTable: "ProductOptions",
                principalColumn: "ProductOptionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOptions_Product_ProductId",
                table: "ProductOptions",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
