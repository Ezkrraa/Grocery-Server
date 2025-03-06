using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grocery_Server.Migrations
{
    /// <inheritdoc />
    public partial class Recipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Groups_GroupId",
                table: "Recipe");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeItem_GroceryItems_ItemId",
                table: "RecipeItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeItem_Recipe_RecipeId",
                table: "RecipeItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeItem",
                table: "RecipeItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe");

            migrationBuilder.RenameTable(
                name: "RecipeItem",
                newName: "RecipeItems");

            migrationBuilder.RenameTable(
                name: "Recipe",
                newName: "Recipes");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeItem_ItemId",
                table: "RecipeItems",
                newName: "IX_RecipeItems_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_GroupId",
                table: "Recipes",
                newName: "IX_Recipes_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeItems",
                table: "RecipeItems",
                columns: new[] { "RecipeId", "ItemId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeItems_GroceryItems_ItemId",
                table: "RecipeItems",
                column: "ItemId",
                principalTable: "GroceryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeItems_Recipes_RecipeId",
                table: "RecipeItems",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Groups_GroupId",
                table: "Recipes",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeItems_GroceryItems_ItemId",
                table: "RecipeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeItems_Recipes_RecipeId",
                table: "RecipeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Groups_GroupId",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeItems",
                table: "RecipeItems");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "Recipe");

            migrationBuilder.RenameTable(
                name: "RecipeItems",
                newName: "RecipeItem");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_GroupId",
                table: "Recipe",
                newName: "IX_Recipe_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeItems_ItemId",
                table: "RecipeItem",
                newName: "IX_RecipeItem_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeItem",
                table: "RecipeItem",
                columns: new[] { "RecipeId", "ItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Groups_GroupId",
                table: "Recipe",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeItem_GroceryItems_ItemId",
                table: "RecipeItem",
                column: "ItemId",
                principalTable: "GroceryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeItem_Recipe_RecipeId",
                table: "RecipeItem",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
