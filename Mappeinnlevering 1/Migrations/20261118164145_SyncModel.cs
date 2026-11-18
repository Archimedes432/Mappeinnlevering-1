using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mappeinnlevering_1.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangeDate",
                table: "PriceChanges",
                newName: "ChangedDate");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Listings",
                newName: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangedDate",
                table: "PriceChanges",
                newName: "ChangeDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Listings",
                newName: "CreateDate");
        }
    }
}
