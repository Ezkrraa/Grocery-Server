using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grocery_Server.Migrations
{
    /// <inheritdoc />
    public partial class requestListItems2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestListItem_GroceryItems_ItemId",
                table: "RequestListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestListItem_Groups_GroupId",
                table: "RequestListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestListItem",
                table: "RequestListItem");

            migrationBuilder.RenameTable(
                name: "RequestListItem",
                newName: "RequestListItems");

            migrationBuilder.RenameIndex(
                name: "IX_RequestListItem_ItemId",
                table: "RequestListItems",
                newName: "IX_RequestListItems_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestListItems",
                table: "RequestListItems",
                columns: new[] { "GroupId", "ItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RequestListItems_GroceryItems_ItemId",
                table: "RequestListItems",
                column: "ItemId",
                principalTable: "GroceryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestListItems_Groups_GroupId",
                table: "RequestListItems",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestListItems_GroceryItems_ItemId",
                table: "RequestListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestListItems_Groups_GroupId",
                table: "RequestListItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestListItems",
                table: "RequestListItems");

            migrationBuilder.RenameTable(
                name: "RequestListItems",
                newName: "RequestListItem");

            migrationBuilder.RenameIndex(
                name: "IX_RequestListItems_ItemId",
                table: "RequestListItem",
                newName: "IX_RequestListItem_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestListItem",
                table: "RequestListItem",
                columns: new[] { "GroupId", "ItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RequestListItem_GroceryItems_ItemId",
                table: "RequestListItem",
                column: "ItemId",
                principalTable: "GroceryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestListItem_Groups_GroupId",
                table: "RequestListItem",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
