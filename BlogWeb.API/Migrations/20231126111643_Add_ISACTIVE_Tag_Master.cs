using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWeb.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_ISACTIVE_Tag_Master : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tags");
        }
    }
}
