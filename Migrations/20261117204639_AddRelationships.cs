using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mappeinnlevering_1.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_BuyerId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_BuyerId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Favorites");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites");

            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                table: "Favorites",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_BuyerId",
                table: "Favorites",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_BuyerId",
                table: "Favorites",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
