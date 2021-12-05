using Microsoft.EntityFrameworkCore.Migrations;

namespace RssSE.Cart.API.Migrations
{
    public partial class CascadeDeleteCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ClientsCarts_CartId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ClientsCarts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "ClientsCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ClientsCarts_CartId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ClientsCarts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "ClientsCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
