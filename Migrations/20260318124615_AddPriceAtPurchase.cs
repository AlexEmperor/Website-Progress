using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_Progress.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceAtPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceAtPurchase",
                table: "CartItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceAtPurchase",
                table: "CartItems");
        }
    }
}
