using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeShare.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewTableForInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedInvitation",
                table: "ShopShares");

            migrationBuilder.DropColumn(
                name: "ShopKey",
                table: "ShopShares");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ShopShares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ShopShares",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShopInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitationMessage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InvitationStatus = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitedUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvitingUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopInvitations_AspNetUsers_InvitedUserId",
                        column: x => x.InvitedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopInvitations_AspNetUsers_InvitingUserId",
                        column: x => x.InvitingUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopInvitations_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "ShopId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopInvitations_InvitedUserId",
                table: "ShopInvitations",
                column: "InvitedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopInvitations_InvitingUserId",
                table: "ShopInvitations",
                column: "InvitingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopInvitations_ShopId",
                table: "ShopInvitations",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopInvitations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ShopShares");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ShopShares");

            migrationBuilder.AddColumn<bool>(
                name: "AcceptedInvitation",
                table: "ShopShares",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShopKey",
                table: "ShopShares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
