using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grocery_Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRecipePictureTakenAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TakenAt",
                table: "RecipePictures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TakenAt",
                table: "RecipePictures",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
