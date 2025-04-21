using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grocery_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Steps",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Steps",
                table: "Recipes");
        }
    }
}
