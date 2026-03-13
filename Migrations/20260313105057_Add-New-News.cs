using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_Progress.Migrations
{
    /// <inheritdoc />
    public partial class AddNewNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortText",
                table: "News");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortText",
                table: "News",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
