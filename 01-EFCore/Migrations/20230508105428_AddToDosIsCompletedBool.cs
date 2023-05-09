using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _01_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddToDosIsCompletedBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "ToDos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "ToDos");
        }
    }
}
