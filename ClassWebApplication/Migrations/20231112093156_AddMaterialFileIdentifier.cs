using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialFileIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaterialFileIdentifier",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialFileIdentifier",
                table: "Courses");
        }
    }
}
