using Microsoft.EntityFrameworkCore.Migrations;

namespace RssSE.Cart.API.Migrations
{
    public partial class Voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "ClientsCarts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "VoucherApplyed",
                table: "ClientsCarts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VoucherCode",
                table: "ClientsCarts",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                table: "ClientsCarts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "ClientsCarts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "ClientsCarts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ClientsCarts");

            migrationBuilder.DropColumn(
                name: "VoucherApplyed",
                table: "ClientsCarts");

            migrationBuilder.DropColumn(
                name: "VoucherCode",
                table: "ClientsCarts");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "ClientsCarts");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "ClientsCarts");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "ClientsCarts");
        }
    }
}
