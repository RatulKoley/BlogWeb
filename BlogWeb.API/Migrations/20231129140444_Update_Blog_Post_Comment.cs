using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWeb.API.Migrations
{
    /// <inheritdoc />
    public partial class Update_Blog_Post_Comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BlogPostComments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogPostComments");
        }
    }
}
