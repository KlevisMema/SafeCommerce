using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeShare.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropsToModerationhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "ModerationHistories");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "ModerationHistories",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ShopId",
                table: "ModerationHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModerationHistories_ShopId",
                table: "ModerationHistories",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationHistories_Shops_ShopId",
                table: "ModerationHistories",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModerationHistories_Shops_ShopId",
                table: "ModerationHistories");

            migrationBuilder.DropIndex(
                name: "IX_ModerationHistories_ShopId",
                table: "ModerationHistories");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "ModerationHistories");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "ModerationHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "ModerationHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
