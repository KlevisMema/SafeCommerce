using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeShare.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelationsShipInModerationHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ModerationHistoryId",
                table: "Shops",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModerationHistoryId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ModerationHistories_ItemId",
                table: "ModerationHistories",
                column: "ItemId",
                unique: true,
                filter: "[ItemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationHistories_ShopId",
                table: "ModerationHistories",
                column: "ShopId",
                unique: true,
                filter: "[ShopId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationHistories_Items_ItemId",
                table: "ModerationHistories",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationHistories_Shops_ShopId",
                table: "ModerationHistories",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "ModerationHistoryId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ModerationHistoryId",
                table: "Items");
        }
    }
}
