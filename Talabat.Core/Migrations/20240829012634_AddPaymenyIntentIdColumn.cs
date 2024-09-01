using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymenyIntentIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "orders",
                newName: "PaymentIntentId");

            migrationBuilder.AddColumn<string>(
                name: "BuyerEmail",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerEmail",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "PaymentIntentId",
                table: "orders",
                newName: "Name");
        }
    }
}
