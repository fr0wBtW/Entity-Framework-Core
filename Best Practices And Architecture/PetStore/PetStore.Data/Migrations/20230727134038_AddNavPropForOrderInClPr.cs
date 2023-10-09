using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNavPropForOrderInClPr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProduct_Order_OrderId",
                table: "ClientProduct");

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "ClientProduct",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProduct_Order_OrderId",
                table: "ClientProduct",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProduct_Order_OrderId",
                table: "ClientProduct");

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "ClientProduct",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProduct_Order_OrderId",
                table: "ClientProduct",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
