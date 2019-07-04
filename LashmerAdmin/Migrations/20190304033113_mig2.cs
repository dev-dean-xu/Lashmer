using Microsoft.EntityFrameworkCore.Migrations;

namespace LashmerAdmin.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCoupon",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    Coupon = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCoupon", x => new { x.UserId, x.Coupon });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCoupon");
        }
    }
}
