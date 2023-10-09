using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetUpProductNameAlternateKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_Name",
                table: "Products",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_Name",
                table: "Products");
        }
    }
}
