using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grocery_Server.Migrations
{
    /// <inheritdoc />
    public partial class DeleteGroupNoAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroceryCategories_Groups_GroupId",
                table: "GroceryCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_OwnerId",
                table: "Groups");

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryCategories_Groups_GroupId",
                table: "GroceryCategories",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroceryCategories_Groups_GroupId",
                table: "GroceryCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_OwnerId",
                table: "Groups");

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryCategories_Groups_GroupId",
                table: "GroceryCategories",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
