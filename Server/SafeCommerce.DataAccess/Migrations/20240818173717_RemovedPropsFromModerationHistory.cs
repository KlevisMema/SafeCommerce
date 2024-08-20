using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeShare.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPropsFromModerationHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModerationHistories_AspNetUsers_ModeratorId",
                table: "ModerationHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ModerationHistories_Items_ItemId",
                table: "ModerationHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ModerationHistories_Shops_ShopId",
                table: "ModerationHistories");

            migrationBuilder.DropIndex(
                name: "IX_ModerationHistories_ItemId",
                table: "ModerationHistories");

            migrationBuilder.DropIndex(
                name: "IX_ModerationHistories_ShopId",
                table: "ModerationHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationHistories_AspNetUsers_ModeratorId",
                table: "ModerationHistories",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModerationHistories_AspNetUsers_ModeratorId",
                table: "ModerationHistories");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationHistories_ItemId",
                table: "ModerationHistories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationHistories_ShopId",
                table: "ModerationHistories",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationHistories_AspNetUsers_ModeratorId",
                table: "ModerationHistories",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationHistories_Items_ItemId",
                table: "ModerationHistories",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationHistories_Shops_ShopId",
                table: "ModerationHistories",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId");
        }
    }
}
