using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grocery_Server.Migrations
{
    /// <inheritdoc />
    public partial class deletionbehaviorforhouseholds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_HouseHolds_HouseHoldId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_HouseHolds_AspNetUsers_OwnerId",
                table: "HouseHolds");

            migrationBuilder.CreateTable(
                name: "HouseholdInvites",
                columns: table => new
                {
                    HouseholdId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdInvites", x => new { x.UserId, x.HouseholdId });
                    table.ForeignKey(
                        name: "FK_HouseholdInvites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseholdInvites_HouseHolds_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "HouseHolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdInvites_HouseholdId",
                table: "HouseholdInvites",
                column: "HouseholdId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_HouseHolds_HouseHoldId",
                table: "AspNetUsers",
                column: "HouseHoldId",
                principalTable: "HouseHolds",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HouseHolds_AspNetUsers_OwnerId",
                table: "HouseHolds",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_HouseHolds_HouseHoldId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_HouseHolds_AspNetUsers_OwnerId",
                table: "HouseHolds");

            migrationBuilder.DropTable(
                name: "HouseholdInvites");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_HouseHolds_HouseHoldId",
                table: "AspNetUsers",
                column: "HouseHoldId",
                principalTable: "HouseHolds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HouseHolds_AspNetUsers_OwnerId",
                table: "HouseHolds",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
