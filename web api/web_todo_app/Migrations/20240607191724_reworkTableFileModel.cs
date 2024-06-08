using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_todo_app.Migrations
{
    /// <inheritdoc />
    public partial class reworkTableFileModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Files",
                newName: "RelativePath");

            migrationBuilder.AddColumn<string>(
                name: "GeneratedName",
                table: "Files",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratedName",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "RelativePath",
                table: "Files",
                newName: "Path");
        }
    }
}
