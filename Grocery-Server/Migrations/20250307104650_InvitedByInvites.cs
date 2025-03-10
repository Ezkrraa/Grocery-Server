using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grocery_Server.Migrations
{
    /// <inheritdoc />
    public partial class InvitedByInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitedBy",
                table: "GroupInvites",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitedBy",
                table: "GroupInvites");
        }
    }
}
