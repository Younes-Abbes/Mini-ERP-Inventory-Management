using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mini_ERP.Migrations
{
    /// <inheritdoc />
    public partial class isdeletedfalse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "categories");
        }
    }
}
