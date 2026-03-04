using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_Progress.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOnMainPageToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnMainPage",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnMainPage",
                table: "Products");
        }
    }
}
