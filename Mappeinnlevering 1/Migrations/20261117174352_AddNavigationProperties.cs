using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mappeinnlevering_1.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                table: "Favorites",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceChanges_ListingId",
                table: "PriceChanges",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_BuyerId",
                table: "Offers",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ListingId",
                table: "Offers",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CategoryId",
                table: "Listings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_SellerId",
                table: "Listings",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_BuyerId",
                table: "Favorites",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ListingId",
                table: "Favorites",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_BuyerId",
                table: "Favorites",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Listings_ListingId",
                table: "Favorites",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AspNetUsers_SellerId",
                table: "Listings",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Categories_CategoryId",
                table: "Listings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_BuyerId",
                table: "Offers",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Listings_ListingId",
                table: "Offers",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceChanges_Listings_ListingId",
                table: "PriceChanges",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_BuyerId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Listings_ListingId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AspNetUsers_SellerId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Categories_CategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_BuyerId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Listings_ListingId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceChanges_Listings_ListingId",
                table: "PriceChanges");

            migrationBuilder.DropIndex(
                name: "IX_PriceChanges_ListingId",
                table: "PriceChanges");

            migrationBuilder.DropIndex(
                name: "IX_Offers_BuyerId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ListingId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Listings_CategoryId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_SellerId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_BuyerId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_ListingId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Favorites");
        }
    }
}
