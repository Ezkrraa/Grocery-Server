using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grocery_Server.Migrations
{
    /// <inheritdoc />
    public partial class NonOptionalRecipeID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_AspNetUsers_UserId",
                table: "ProfilePictures");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_AspNetUsers_UserId",
                table: "ProfilePictures",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_AspNetUsers_UserId",
                table: "ProfilePictures");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_AspNetUsers_UserId",
                table: "ProfilePictures",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
